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
using System.Drawing;
using System.Windows.Forms;

namespace Diagram.DiagramView
{
    class RubberBand
    {
        private readonly Control diagram;
        private readonly View view;
        private readonly Pin pin;
        private readonly Point lineStart;
        private Point lineEnd;
        private readonly Pen validPen;
        private readonly Pen invalidPen;

        private bool validConnection;

        public RubberBand(Control diagram, View view, Pin pin)
        {
            this.pin = pin;
            this.diagram = diagram;
            this.view = view;
            lineStart = pin.Centre;
            lineStart.Offset(pin.Shape.Location);
            lineEnd = Point.Empty;
            validPen = new Pen(Color.Black, 2f);
            invalidPen = new Pen(Color.Red, 1f) {DashPattern = new[] {4.0F, 4.0F}};
        }

        public void Render(Graphics g)
        {
            g.DrawLine(validConnection ? validPen : invalidPen, lineStart, lineEnd);
        }

        public void MouseMove(Point p)
        {
            var extents = GetExtents(lineStart, p);

            if(lineEnd != Point.Empty)
            {
                var oldExtents = GetExtents(lineStart, lineEnd);

                extents = Rectangle.Union(oldExtents, extents);
            }
            lineEnd = p;
            validConnection = false;
            var obj = view.HitTest(p);
            if(obj is Pin)
                validConnection = (obj as Pin).Flow == Flow.InFlow ^ pin.Flow == Flow.InFlow;
            diagram.Invalidate(extents);

        }

        public Connector MouseUp(Point p)
        {
            var extents = GetExtents(lineStart, lineEnd);
            diagram.Invalidate(extents);
            var obj = view.HitTest(p);
            if(obj is Pin && (obj as Pin).Flow == Flow.InFlow ^ pin.Flow == Flow.InFlow)
            {
                Pin from, to;
                if(pin.Flow == Flow.InFlow)
                {
                    from = obj as Pin;
                    to = pin;
                }
                else
                {
                    from = pin;
                    to = obj as Pin;
                }
                var sourceName = from.Shape.FindPin(from);
                var flowObj = view.Model.FindOutflow(pin.Shape.Task, sourceName);
                var conn = Connector.Create(flowObj, from, to);
                flowObj.Connector = conn;
                flowObj.TargetTask = conn.TaskTo;
                flowObj.TargetPin = "In";
                conn.UnRoute();
                view.Model.RouteConnection(conn);
            }
            return null;
        }

        public static Rectangle GetExtents(Point start, Point end)
        {
            return new Rectangle
            {
                X = Math.Min(start.X, end.X) - 1,
                Y = Math.Min(start.Y, end.Y) - 1,
                Width = Math.Abs(start.X - end.X) + 2,
                Height = Math.Abs(start.Y - end.Y) + 2
            };
        }




    }
}
