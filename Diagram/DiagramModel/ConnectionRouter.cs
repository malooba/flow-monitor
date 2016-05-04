//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Diagram.DiagramView;
using View = Diagram.DiagramView.View;


namespace Diagram.DiagramModel
{
    public class ConnectionRouter
    {
        private Rectangle routingSpace;
        private readonly RouteMap map;
        private MapRef target;
        private PriorityQueue queue;

        // The tentative points created while routing
        private Dictionary<MapRef, RoutePoint> routePoints;

        // Existing connections that have been mapped onto the routing grid
        private Dictionary<MapRef, RoutedPoint> routedPoints; 

        // This is the amount that the routing grid extends above and below the diagram bounds to allow room for new routes
        public static int RoutingMargin = 40;

        // Routing penalties 
        // These values are penalties per crossing and per bend
        // They are expressed in distance grid units
        // i.e. if the BendPenalty is x then each bend costs the same as an additional routing length of x grid units
        public static double CrossingPenalty = 3.0;
        public static double BendPenalty = 3.0;

        // Set this true to ensure that only two flows can join at a single point (like a T-junction)
        // If this is false then three flows can meet at a point which might be confused with a crossing
        public static bool TwoWayJoins = true;

        /// <summary>
        /// Constructor
        /// The Routing map is initialised with HardBlocks covering the shapes.
        /// It is also necessary to call MapConnectors() to initialise the RoutedPoints Dictionary with the existing connections.
        /// </summary>
        /// <param name="model"></param>
        public ConnectionRouter(Model model)
        {
            routingSpace = View.RoundOutToGrid(model.GetDiagramBounds(RoutingMargin));

            // Create the map of routing points on the grid spacing
            map = new RouteMap(routingSpace.Width / View.GRID_SIZE, routingSpace.Height / View.GRID_SIZE);

            foreach(var task in Model.Workflow.Tasks)
            {
                var shape = task.Symbol.Shape;
                var bounds = View.RoundOutToGrid(shape.Bounds);
                bounds.X -= routingSpace.X;
                bounds.Y -= routingSpace.Y;
                for(var x = bounds.Left / 10; x <= bounds.Right / 10; x++)
                    for(var y = bounds.Top / 10; y <= bounds.Bottom / 10; y++)
                        map[x, y] = RouteState.HardBlock;

                // Make holes
                foreach(var pin in shape.pins.Values)
                {
                    var m = SnapToMap(shape.Bounds.X + pin.Centre.X, shape.Bounds.Y + pin.Centre.Y);
                    while(map.Inside(m) && map[m] == RouteState.HardBlock)
                    {
                        map[m] = RouteState.Empty;
                        m = m.Step(pin.Direction);
                    }
                }
            }
        }

        /// <summary>
        ///  Map all existing connections
        /// </summary>
        public void MapConnectors()
        {
            routedPoints = new Dictionary<MapRef, RoutedPoint>();
            foreach(var conn in Connector.Connectors.Where(c => c.Routed))
                MapConnector(conn);
        }

        /// <summary>
        /// Add the routed connector to the map as a chain of SoftBlock points 
        /// </summary>
        /// <param name="connector"></param>
        private void MapConnector(Connector connector)
        {
            if(connector.Points.Count < 2)
                throw new ApplicationException("Not enough points in connector");
            var mapRefs = connector.Points.Select(p => SnapToMap(p.X, p.Y)).ToArray();
            var last = mapRefs.Last();
            var dir = Direction.Create(mapRefs[0], mapRefs[1]);
            var prev = MapRef.Empty;

            foreach(var m in TraceConnector(mapRefs))
            {
                if(prev != MapRef.Empty)
                    dir = Direction.Create(prev, m);
                AddRoutedPoint(m, last, dir);
                prev = m;
            }
            AddRoutedPoint(last, last, dir);
        }

        private static IEnumerable<MapRef> TraceConnector(MapRef[] mapRefs)
        {
            var x = mapRefs[0];
            foreach(var y in mapRefs.Skip(1))
            {
                var i = Interpolate(x, y);
                foreach(var m in i)
                    yield return m;
                x = y;
            }
            yield return mapRefs.Last();
        }

        private void AddRoutedPoint(MapRef m, MapRef targ, Direction dir)
        {
            RoutedPoint routedPoint;
            if(routedPoints.TryGetValue(m, out routedPoint))
            {
                if(routedPoint.Target != targ)
                    routedPoints[m] = new RoutedPoint();       // RoutedPoint cross (different targets)
                else if(routedPoint.Direction != dir)
                    routedPoints[m] = new RoutedPoint(targ);   // RoutedPoint join (same target from different directions)
            }
            else
            {
                routedPoints[m] = new RoutedPoint(targ, dir);
            }
        }

        /// <summary>
        /// Interpolate over [from, to) in the map
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static IEnumerable<MapRef> Interpolate(MapRef from, MapRef to)
        {
            var dir = Direction.Create(from, to);


            var i = from;
            while(i.X != to.X || i.Y != to.Y)
            {
                // Debug.WriteLine("MapRef = {0}, {1}", i.X, i.Y);
                yield return i;
                i = i.Step(dir);
            }
        }

        /// <summary>
        /// Route a single connection
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public bool RouteConnection(Connector conn)
        {
            Debug.WriteLine("Connecting {0}:{1} to {2}:{3}", conn.TaskFrom.TaskId, conn.Flow.Name, conn.Flow.Target, conn.Flow.TargetPin);

            var start = SnapToMap(conn.TaskFrom.Symbol.Shape.Bounds.X + conn.PinFrom.Centre.X, conn.TaskFrom.Symbol.Shape.Bounds.Y + conn.PinFrom.Centre.Y);
            target = SnapToMap(conn.TaskTo.Symbol.Shape.Bounds.X + conn.PinTo.Centre.X, conn.TaskTo.Symbol.Shape.Bounds.Y + conn.PinTo.Centre.Y);
            Debug.WriteLine("Coordinates ({0}, {1}) to ({2}, {3})", start.X, start.Y, target.X, target.Y);
            map.ClearVisitedFlags();
            queue = new PriorityQueue(200);
            routePoints = new Dictionary<MapRef, RoutePoint>();
            var initial = new RoutePoint(start.X, start.Y)
            {
                Direction = conn.PinFrom.Direction,
                Heuristic = Manhatten(start, target)
            };
            queue.Enqueue(initial);

            RoutePoint route = null;
            while(route == null)
            {
                if(queue.Count == 0)
                {
                    Debug.WriteLine("FAILED TO CONNECT {0}:{1} to {2}:{3}", conn.TaskFrom.TaskId, conn.Flow.Name, conn.Flow.Target, conn.Flow.TargetPin);
                    return false;
                }
               route = Astar();
            }

            var connectorPoints = new List<Point>();

            var trace = route;

            connectorPoints.Add(MapToDiagram(trace.X, trace.Y));
            while(trace != null)
            {
                AddRoutedPoint(trace.Location, target, trace.Direction);
                
                if(trace.Previous == null)
                    connectorPoints.Add(MapToDiagram(trace.X, trace.Y));
                else if(trace.Previous.Direction != trace.Direction)
                    connectorPoints.Add(MapToDiagram(trace.Previous.X, trace.Previous.Y));

                trace = trace.Previous;
            }
            connectorPoints.Reverse();
            conn.Points = connectorPoints;
            return true;
        }

        /// <summary>
        /// A* algorithm to route connections
        /// Returns the target RoutePoint upon success or null upon failure.
        /// The route can be determined by following the Previous pointers from
        /// RoutePoint to RoutePoint.  Changes in direction provide a simple way
        /// to detect the bends in the route so as to reduce it to straight line segments
        /// </summary>
        /// <returns>The target route point</returns>
        private RoutePoint Astar()
        {
            var current = queue.Dequeue();
            routePoints.Remove(current.Location);

            // Debug.WriteLine("Dequeued {0}, {1}", current.X, current.Y);
            if(current.Location == target)
                return current;

            foreach(var dir in Direction.All())
            {
                var mr = current.Location.Step(dir);
                var mapState = map[mr];

                if(mapState != RouteState.Empty)
                    continue;

                RoutedPoint routedPoint;

                if(routedPoints.TryGetValue(mr, out routedPoint))
                {
                    if(routedPoint.Target == MapRef.Empty) // This indicates an existing crossing point
                        continue;
                }

                var crossing = routedPoint != null && routedPoint.Target != target;

                RoutedPoint currentRoutedPoint;
                if(routedPoints.TryGetValue(current.Location, out currentRoutedPoint))
                {
                    // No changes of direction when crossing an existing route
                    if(currentRoutedPoint.Target != target && dir != current.Direction) continue;

                    // Don't try a direction that doesn't match the one for the existing trace
                    if(currentRoutedPoint.Target == target && dir != currentRoutedPoint.Direction && currentRoutedPoint.Direction != Direction.Empty) continue;

                    // Don't follow existing routed point for the wrong target 
                    if(currentRoutedPoint.Target != target && routedPoint?.Target == currentRoutedPoint.Target)
                        continue;
                }

                // Don't join at an existing join point if TwoWayJoins is set
                if(TwoWayJoins && routedPoint != null && routedPoint.Direction == Direction.Empty && currentRoutedPoint?.Target != target ) 
                   continue;


                var existing = true;
                RoutePoint n;
                if(!routePoints.TryGetValue(mr, out n))
                {
                    n = new RoutePoint(mr.X, mr.Y);
                    existing = false;
                }
                var bends = current.Bends;
                if(current.Previous != null && current.Previous.X != mr.X && current.Previous.Y != mr.Y) bends++;
                var crossings = current.Crossings + (crossing ? 1 : 0);
                var length = current.Length + 1;
                var cost = length + crossings * CrossingPenalty;
                var bendEstimate = MinBendEstimate(n, dir);
                if(!existing || cost + bendEstimate * BendPenalty < n.Cost + n.BendEstimate * BendPenalty)
                {
                    n.Cost = cost;
                    n.Length = length;
                    n.Bends = bends;
                    n.Crossings = crossings;
                    n.Previous = current;
                    n.BendEstimate = bendEstimate;
                    n.Heuristic = Manhatten(n.Location, target) + bendEstimate * BendPenalty;
                    n.Direction = dir;
                    n.Priority = n.Cost + n.Heuristic;
                }
                if(existing)
                {
                    queue.UpdatePriority(n);
                }
                else
                {
                    queue.Enqueue(n);
                    routePoints.Add(n.Location, n);
                }

                // Debug.WriteLine("Enqueued {0}, {1} total cost = {2}", n.X, n.Y, n.Cost + n.Heuristic);
            }

            map[current.Location] = RouteState.Visited;
            return null;
        }

        /// <summary>
        /// Estimate the minimum number of bends in the remaining route
        /// This is the actual number of bends assuming no obstacles
        /// </summary>
        /// <param name="start"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private int MinBendEstimate(RoutePoint start, Direction dir)
        {
            var inline =
                dir.Vertical && start.Y == target.Y ||
                dir.Horizontal && start.X == target.X;

            var dx = target.X - start.X;
            var dy = target.Y - start.Y;

            var ahead =
                dx > 0 && dir == Direction.East ||
                dy > 0 && dir == Direction.South ||
                dx < 0 && dir == Direction.West ||
                dy < 0 && dir == Direction.North;

            return inline ? (ahead ? 0 : 3) : (ahead ? 1 : 2);

        }

        /// <summary>
        /// Render the map for debugging
        /// </summary>
        /// <param name="g"></param>
        public void RenderMap(Graphics g)
        {
            for(var x = 0; x < map.Width; x++)
            {
                for(var y = 0; y < map.Height; y++)
                {
                    var rs = map[x, y];
                    Brush b = null;
                    if(rs == RouteState.HardBlock)
                        b = Brushes.Red;

                    else if(rs == RouteState.Visited)
                        b = Brushes.Yellow;

                    else if(routedPoints.ContainsKey(new MapRef(x, y)))
                        b = Brushes.SpringGreen;

                    if(b != null)
                    {
                        var p = MapToDiagram(x, y);
                        g.FillEllipse(b, p.X - 2, p.Y - 2, 5, 5);
                    }
                }
            }

        }

        /// <summary>
        /// Convert diagram point to map indices
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        private MapRef SnapToMap(int px, int py)                                             
        {
            var x = px - routingSpace.X;
            var y = py - routingSpace.Y;
            //if(x % Model.GridSize != 0 || y % Model.GridSize != 0)
                //Debug.WriteLine("Point [{0}, {1}] not on grid", px, py);

            return new MapRef(
               ((2 * x + View.GRID_SIZE) / (2 * View.GRID_SIZE)),
               ((2 * y + View.GRID_SIZE) / (2 * View.GRID_SIZE))
            );
        }

        private Point MapToDiagram(int x, int y)
        {
            return new Point
            {
                X = (x * View.GRID_SIZE) + routingSpace.X,
                Y = (y * View.GRID_SIZE) + routingSpace.Y
            };
        }

        /// <summary>
        /// Manhatten distance (Distance a rook moves on a chessboard)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private int Manhatten(MapRef from, MapRef to)
        {
            return(Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y));
        }
    }
}
