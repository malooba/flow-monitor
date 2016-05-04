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
using System.Diagnostics;

namespace Diagram.DiagramModel
{
    public struct MapRef : IEquatable<MapRef>
    {
        public readonly int X;
        public readonly int Y;

        public static readonly MapRef Empty = new MapRef(-1, -1);

        public MapRef(int x, int y)
        {
            if(x < 0 || y < 0)
            {
                X = -1;
                Y = -1;
            }
            else
            {
                X = x;
                Y = y;
            }
        }

        public MapRef Step(Direction dir)
        {
            Debug.Assert(dir != Direction.Empty);

            return new MapRef(X + dir.DX, Y + dir.DY);
        }

        public bool Equals(MapRef other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is MapRef && Equals((MapRef)obj);
        }

        public static bool operator ==(MapRef m1, MapRef m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(MapRef m1, MapRef m2)
        {
            return !m1.Equals(m2);
        }

        public override int GetHashCode()
        {
            return unchecked((X << 16) ^ (Y & (int)0XFFFF0000)) | ((Y & 0xFFFF) ^ (X >> 16));
        }
    }
}
