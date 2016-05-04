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
    /// <summary>
    /// A grid point that already contains one or more completed routes
    /// These are stored in the Dictionary routedPoints keyed by the points MapRef
    /// </summary>
    class RoutedPoint
    {
        /// <summary>
        /// The MapRef of the target(s) of the route(s) passing through.
        /// This will be MapRef.Empty if two routes cross.
        /// </summary>
        public readonly MapRef Target;

        /// <summary>
        /// The common direction of the route(s) passing through. 
        /// This will be Direction.Empty if the directions conflict.
        /// </summary>
        public readonly Direction Direction;

        /// <summary>
        /// Construct a crossing point.
        /// Two routes that have nothing in common.
        /// </summary>
        public RoutedPoint()
        {
            Target = MapRef.Empty;
            Direction = Direction.Empty;
        }

        /// <summary>
        /// Construct a join point.
        /// Two routes to a common target from different directions
        /// </summary>
        /// <param name="target"></param>
        public RoutedPoint(MapRef target)
        {
            Target = target;
            Direction = Direction.Empty;
        }

        /// <summary>
        /// One or more routes with a common target and direction
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        public RoutedPoint(MapRef target, Direction direction)
        {
            Target = target;
            Direction = direction;
        }
    }
}