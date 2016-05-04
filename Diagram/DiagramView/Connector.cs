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
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    public class Connector : Visible
    {
        private static readonly List<Connector> connectors = new List<Connector>();
        public static readonly IReadOnlyList<Connector> Connectors = connectors;
       
        public readonly FlowObj Flow;
        public readonly TaskObj TaskFrom;
        public readonly TaskObj TaskTo;
        public readonly Pin PinFrom;
        public readonly Pin PinTo;

        private static float penWidth = 3f;
        private static readonly Pen selectedPen = new Pen(Color.OrangeRed , penWidth);
        private static readonly Pen pen = new Pen(Color.Black , penWidth);

        public IReadOnlyList<Point> Points
        {
            get { return points; }
            set
            {
                points = (List<Point>)value;
                Bounds = GetBounds();
            }
        }
        private List<Point> points;

        private const int HitMargin = 5;

        public static Connector Create(FlowObj flow, Pin pinFrom, Pin pinTo)
        {
            var taskFrom = pinFrom.Shape.Task;
            var taskTo = pinTo.Shape.Task;
            return connectors.SingleOrDefault(c => c.TaskFrom == taskFrom && c.PinFrom == pinFrom) ??
               new Connector(flow, taskFrom, pinFrom, taskTo, pinTo);
        }

        private Connector(FlowObj flow, TaskObj taskFrom, Pin pinFrom, TaskObj taskTo, Pin pinTo)
        {
            Flow = flow;
            TaskFrom = taskFrom;
            PinFrom = pinFrom;
            TaskTo = taskTo;
            PinTo = pinTo;
            if(flow.Route != null)
            {
                points = new List<Point>();
                var iterator = flow.Route.GetEnumerator();
                while(iterator.MoveNext())
                {
                    var x = (int)iterator.Current;
                    if(!iterator.MoveNext())
                    {
                        Debug.WriteLine("Odd number of route coordinates, ignoring - {0}:{1}", taskFrom.TaskId, flow.Name);
                        Points = null;
                        break;
                    }
                    var y = (int)iterator.Current;
                    points.Add(new Point(x, y));
                }
                
            }
            Bounds = GetBounds();
            connectors.Add(this);
        }

        private Rectangle GetBounds()
        {
            if(!Routed) return Rectangle.Empty;
            var left = int.MaxValue;
            var top = int.MaxValue;
            var right = int.MinValue;
            var bottom = int.MinValue;

            foreach(var p in points)
            {
                if(p.X < left) left = p.X;
                if(p.X > right) right = p.X;
                if(p.Y < top) top = p.Y;
                if(p.Y > bottom) bottom = p.Y;
            }
            return Rectangle.FromLTRB(left - HitMargin, top - HitMargin, right + HitMargin, bottom + HitMargin);
        }

        /// <summary>
        /// Get the route as an array of alternate X and Y coordinates
        /// This is the form stored in the workflow JSON
        /// </summary>
        /// <returns></returns>
        public int[] GetRoute()
        {
            if(!Routed) return null;
            var route = new List<int>();
            foreach(var p in points)
            {
                route.Add(p.X);
                route.Add(p.Y);
            }
            return route.ToArray();
        }

        /// <summary>
        /// Delete any routing information
        /// </summary>
        public void UnRoute()
        {
            points = null;
        }

        /// <summary>
        /// True if this connector is routed
        /// </summary>
        public bool Routed => Points != null && Points.Count > 1;

        public void Render(Graphics g)
        {
            var p = Selected ? selectedPen : pen;
            g.DrawLines(p, points.ToArray());
        }

        // Get the connectors connected to a task
        public static IEnumerable<Connector> GetConnections(TaskObj task)
        {
            return Connectors.Where(c => c.TaskFrom == task || c.TaskTo == task);
        }

        public void Select(bool selected)
        {
            throw new NotImplementedException();
        }

        public override Visible HitTest(Point point)
        {
            if(!Bounds.Contains(point))
                return null;

            var s = points.First();
            foreach(var e in points.Skip(1))
            {
                if(Math.Abs(point.X - s.X) < HitMargin)
                {
                    if(point.Y < s.Y != point.Y <= e.Y)
                        return new Segment(this, s, e, points);
                }
                else if(Math.Abs(point.Y - s.Y) < HitMargin)
                {
                    if(point.X < s.X != point.X <= e.X)
                        return new Segment(this, s, e, points);
                }
                s = e;
            }
            return null;
        }

        /// <summary>
        /// Determine if a rectangle intersects this connector
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool HitsRectangle(Rectangle rect)
        {
          /*
            To test if a rectangle intersects a connector
            1) Check if any of the connector points lie on the rectangle
            2) Check if any consecutive points lie above and below the rectangle and have a common X coordinate on the horizontal extent of the rectangle
            3) Check if any consecutive points lie left and right of the rectangle and have a common Y coordinate on the vertical extent of the rectangle
          */

            // Test 1
            if(Points.Any(rect.Contains))
                return true;

            var s = points.First();
            foreach(var e in points.Skip(1))
            {
                // Test 2
                if(s.X == e.X)
                {
                    if((s.Y < rect.Top && e.Y > rect.Bottom) || (e.Y < rect.Top && s.Y > rect.Bottom)) // Does this segment vertically bracket the rectangle
                    {
                        if(s.X >= rect.Left && s.X <= rect.Right)                                      // and lie on its horizontal extent?
                            return true;
                    }
                }
                else // Test 3
                {
                    if((s.X < rect.Left && e.X > rect.Right) || (e.X < rect.Left && s.X > rect.Right)) // Does this segment horizontally bracket the rectangle
                    {
                        if(s.Y >= rect.Top && s.Y <= rect.Bottom)                                      // and lie on its vertical extent?
                            return true;
                    }
                }
                s = e;
            }
            return false;
        }

        public void Normalise()
        {
            var indices = new List<int>();

            // scan interior points
            for(var i = 1; i < points.Count - 1; i++)
            {
                if((points[i - 1].X == points[i].X && points[i].X == points[i + 1].X) ||
                   (points[i - 1].Y == points[i].Y && points[i].Y == points[i + 1].Y))

                    indices.Add(i - indices.Count);
            }

            foreach(var index in indices)
                points.RemoveAt(index);

            Bounds = GetBounds();
        }

        public void Delete()
        {
            Flow.TargetTask = null;
            Flow.TargetPin = null;
            Flow.Route = null;
            Flow.Connector = null;
            connectors.Remove(this);
        }

	    public static void ClearConnectors()
	    {
		    connectors.Clear();
	    }
    }
}
