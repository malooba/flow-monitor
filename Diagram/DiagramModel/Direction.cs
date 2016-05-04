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

namespace Diagram.DiagramModel
{
    /// <summary>
    /// Compass directions for auto routing
    /// Note that Y coordinates increase in the southerly direction
    /// </summary>
    public class Direction
    {
        public static readonly Direction North = new Direction("North", 0, -1);
        public static readonly Direction South = new Direction("South", 0, 1);
        public static readonly Direction East = new Direction("East", 1, 0);
        public static readonly Direction West = new Direction("West", -1, 0);
        public static readonly Direction Empty = new Direction("Empty", 0, 0);
        
        public int DX { get; }
        public int DY { get; }

        private readonly string name;

        private Direction(string name, int dx, int dy)
        {
            this.name = name;
            DX = dx;
            DY = dy;
        }

        /// <summary>
        /// Create a direction between two orthogonal points
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Direction Create(MapRef from, MapRef to)
        {
            if(from == to)
                throw new ApplicationException("Direction - points equal");
            if(from.X == to.X)
                return to.Y > from.Y ? South : North;
            if(from.Y == to.Y)
                return to.X > from.X ? East : West;
            throw new ApplicationException("Direction - points not aligned");
        }

        /// <summary>
        /// True for horizontal directions
        /// </summary>
        public bool Horizontal =>  DY == 0;

        /// <summary>
        /// True for vertical directions
        /// </summary>
        public bool Vertical => DX == 0;

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Enumerate the valid directions (excluding Empty)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Direction> All()
        {                
            yield return East;
            yield return North;
            yield return South;
            yield return West;
        }
    }
}
