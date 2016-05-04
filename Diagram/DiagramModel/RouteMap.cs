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
using System.Collections;

namespace Diagram.DiagramModel
{
    /// <summary>
    /// The RouteMap is the grid of points that connectors can route over
    /// Each point on the grid holds two bits, the visited bit and the hardBlocks bit.
    /// Visited points have already been considered by the search algorithm<para/>
    /// A hard block is a symbol or other feature that does not permit the passage of a connector<para/>
    /// There is also the idea of a softBlock which is an already routed point.  These are stored
    /// separately in the existing route definitions. Routing is permitted through a soft block 
    /// (with additional cost) but it is not permitted to route from a soft block to another.
    /// This prevents connectors being overlaid unless orthogonal.
    /// </summary>

    public class RouteMap
    {
        public readonly int Width;
        public readonly int Height;

        private readonly BitArray hardBlocks;
        private readonly BitArray visited;

        /// <summary>
        /// Construct a RouteMap with the given dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RouteMap(int width, int height)
        {
            if(width < 1 || height < 1)
                throw new ApplicationException("Degenerate route map");
            Width = width;
            Height = height;

            var size = Width * Height;
            hardBlocks = new BitArray(size);
            visited = new BitArray(size);
        }

        /// <summary>
        /// Test if a MapRef lies within the extents of this RouteMap
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Inside(MapRef m)
        {
            return m.X >= 0 && m.Y >= 0 && m.X < Width && m.Y < Height;
        }

        /// <summary>
        /// Test if the map coordinates lie within the extents of this RouteMap
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Inside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        /// <summary>
        /// Read / overwrite the map point state
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public RouteState this[int x, int y]
        {
            get
            {
                if(!Inside(x, y))
                    return RouteState.HardBlock;
                var i = Index(x, y);
                return hardBlocks[i] ? 
                       RouteState.HardBlock : 
                       visited[i] ? RouteState.Visited : RouteState.Empty;

            }
            set
            {
                if(!Inside(x, y))
                    return; // ignore

                var i = Index(x, y);
                switch(value)
                {
                    case RouteState.Empty:
                        hardBlocks[i] = false;
                        visited[i] = false;
                        break;

                    case RouteState.HardBlock:
                        hardBlocks[i] = true;
                        visited[i] = false;
                        break;

                    case RouteState.Visited:
                        hardBlocks[i] = false;
                        visited[i] = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Get the RouteState of a MapRef in this RouteMap
        /// </summary>
        /// <param name="mr"></param>
        /// <returns></returns>
        public RouteState this[MapRef mr]
        {
            get { return this[mr.X, mr.Y]; }
            set { this[mr.X, mr.Y] = value; }
        }

        /// <summary>
        /// Clear the 'Visited' flags in the map in preparation for another route-finding run
        /// </summary>
        public void ClearVisitedFlags()
        {
           visited.SetAll(false);
        }

        /// <summary>
        /// Convert a 2D map reference into a 1D BitArray reference
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Index(int x, int y)
        {
            return x + Width * y;
        }
    }

    public enum RouteState
    {
        Empty,
        HardBlock,
        Visited
    }
}
