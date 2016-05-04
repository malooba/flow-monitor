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

using System.Drawing;
using System.Windows.Forms;
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    /// <summary>
    /// Pin flow direction
    /// </summary>
    public enum Flow
    {
        InFlow, OutFlow, ErrorFlow
    }

    
    public class Pin : Visible
    {
        public readonly Flow Flow;

        /// <summary>
        /// Pin centre location referred to Symbol
        /// </summary>
        public Point Centre { get; }

        public readonly Shape Shape;

        public readonly Direction Direction;

        private const int Radius = 6;
        private const int R2 = Radius * Radius;

        private static readonly Pen pen = new Pen(Color.Black, 2f);
        private readonly Brush brush;


        public Pin(Shape shape, Flow flow, Point centre)
        {
            Shape = shape;
            Flow = flow;

            // Currently the pin connection direction is determined by the flow direction
            switch(flow)
            {
                case Flow.InFlow:
                    Direction = Direction.West;
                    break;

                case Flow.OutFlow:
                    Direction = Direction.East;
                    break;

                case Flow.ErrorFlow:
                    Direction = Direction.South;
                    break;
            }

            Centre = centre;
            brush = GetBrush();
        }

        private Brush GetBrush()
        {
            switch(Flow)
            {
                case Flow.InFlow:
                    return Brushes.LawnGreen;

                case Flow.OutFlow:
                    return Brushes.LightSkyBlue;

                case Flow.ErrorFlow:
                    return Brushes.Pink;
            }
            // Hopefully won't happen, now or in the fuchsia
            return Brushes.Fuchsia;
        }

        public void Render(Graphics g)
        {
            var rect = new Rectangle
            {
                X = Centre.X - Radius,
                Y = Centre.Y - Radius,
                Width = Radius * 2,
                Height = Radius * 2
            };
            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }

        public override Visible HitTest(Point p)
        {
            var dx = p.X - Centre.X;
            var dy = p.Y - Centre.Y;
            return dx * dx + dy * dy < R2 + 2 ? this : null;
        }

        public override Cursor GetCursor => Cursors.Cross;
    }
}
