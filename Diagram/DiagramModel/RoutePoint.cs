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

namespace Diagram.DiagramModel
{
    public class RoutePoint
    {
        /// <summary>
        /// Point coordinates on routing grid
        /// </summary>
        public MapRef Location { get; }

        /// <summary>
        /// X coordinate on routing grid
        /// </summary>
        public int X => Location.X;

        /// <summary>
        /// Y coordinate on routing grid
        /// </summary>
        public int Y => Location.Y;

        /// <summary>
        /// Estimated cost of remaining route
        /// </summary>
        public double Heuristic { get; set; }

        /// <summary>
        /// Cost of route so far
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Number of crossings so far
        /// </summary>
        public int Crossings { get; set; }

        /// <summary>
        /// Number of bends to this point
        /// </summary>
        public int Bends { get; set; }

        /// <summary>
        /// Estimated number of bends to complete the route
        /// </summary>
        public int BendEstimate { get; set; }

        /// <summary>
        /// Number of grid points between here an the start of the route
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Link to previous route point
        /// </summary>
        public RoutePoint Previous { get; set; }

        /// <summary>
        /// The direction from the previous point
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// The total cost of this point including the estimated completion cost (heuristic) 
        /// </summary>
        public double Priority { get; set; }

        /// <summary>
        /// Incrementing index used to order points with equal priority
        /// Points queued earlier have priority 
        /// </summary>
        public long InsertionIndex { get; set; }

        /// <summary>
        /// Heap index for priority queue
        /// </summary>
        public int QueueIndex { get; set; }

        public RoutePoint(int x, int y)
        {
            Location = new MapRef(x, y);
        }
    }
}
