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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    public class BoxShape : Shape
    {
        private const int CornerRadius = 8;

        // Offset to shape Top Left from Bounds

        internal BoxShape(TaskObj task, Point datumLocation) : base(task)
        {
            InitShape(120, 80, datumLocation);
        }

        protected override void AddPins()
        {
            pins = new Dictionary<string, Pin>();
            if(Task.HasInflow())
                pins.Add("In", new Pin(this, Flow.InFlow, new Point(datum.X, datum.Y + size.Height / 2)));

			if(Task.HasFailOutflow())
				pins.Add("Err", new Pin(this, Flow.ErrorFlow, new Point(datum.X + size.Width / 2, datum.Y + size.Height)));

			if(Task.Outflows == null)
				return;

			if(Task.Outflows.Length > 3)
				throw new ApplicationException("Box shape cannot have more than three outflows");

            // This assumes Symbol is at least 80 high
            var ys = new int[,]
            {
                {0, 0, 0},
                {-20, 20, 0},
                {-20, 0, 20}
            };
            var i = 0;
            if(Task.Outflows != null)
            {
                foreach(var outflow in Task.Outflows)
                {
                    pins.Add(outflow.Name, new Pin(this, Flow.OutFlow, new Point(datum.X + size.Width, datum.Y + ys[Task.Outflows.Length - 1, i++] + size.Height / 2)));
                }
            }
        }

        protected override void Redraw()
        {
            var g = GetImageGraphics();

            var path = RoundedRectanglePath(shapeRect, CornerRadius);
            g.FillPath(Selected ? selectedBrush : brush, path);
            g.DrawPath(pen, path);

            foreach(var pin in pins)
                pin.Value.Render(g);

            var labelMax = shapeRect;
            labelMax.Inflate(-10, -10);

            g.FillRectangle(backBrush, idRect);
            g.DrawString(Task.Symbol.Label, Font, Brushes.Black, labelMax, labelFormat);
            g.DrawString(Task.TaskId, Font, Brushes.Black, idRect, idFormat);
        }

        private static GraphicsPath RoundedRectanglePath(Rectangle rect, int r)
        {
            var path = new GraphicsPath();
            var d = r * 2;
            var arc = new Rectangle(rect.Location, new Size(d, d));
            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - d;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - d;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }


        public override Visible HitTest(Point point)
        {
            // Quick elimination with bounds
            if(!Bounds.Contains(point)) return null;

            // convert to local coordinates
            point.Offset(-Bounds.X, -Bounds.Y);

            // Check Task ID label
            if(idRect.Contains(point))
                return this;

            // Check Pins
            var pin = pins.Values.FirstOrDefault(p => p.HitTest(point) != null);
            if(pin != null)
                return pin;

            // Check shape body
            return shapeRect.Contains(point) ? this : null;

        }
    }
}
