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
using System.Linq;
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    class CircleShape : Shape
    {
        internal CircleShape(TaskObj task, Point datumLocation)
            : base(task)
        {
            InitShape(80, 80, datumLocation);
        }

		protected override void AddPins()
		{
			pins = new Dictionary<string, Pin>();


			if(Task.HasInflow())
				pins.Add("In", new Pin(this, Flow.InFlow, new Point(datum.X, datum.Y + size.Height / 2)));

			if(Task.HasFailOutflow())
				pins.Add("Err", new Pin(this, Flow.OutFlow, new Point(datum.X + size.Width / 2, datum.Y + size.Height)));

			if(Task.Outflows == null) 
				return;

			if(Task.Outflows.Length > 1)
				throw new ApplicationException("Circle shape cannot have more than one outflow");
			if(Task.Outflows.Length == 1)
				pins.Add(Task.Outflows[0].Name, new Pin(this, Flow.OutFlow, new Point(datum.X + size.Width, datum.Y + size.Height / 2)));
		}

        protected override void Redraw()
        {
            var g = GetImageGraphics();

            g.FillEllipse(Selected ? selectedBrush : brush, shapeRect);
            g.DrawEllipse(pen, shapeRect);

            foreach(var pin in pins)
                pin.Value.Render(g);

            var labelMax = shapeRect;
            labelMax.Inflate(-10, -10);

            g.FillRectangle(backBrush, idRect);
            g.DrawString(Task.Symbol.Label, Font, Brushes.Black, labelMax, labelFormat);
            g.DrawString(Task.TaskId, Font, Brushes.Black, idRect, idFormat);
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
            return EllipseHitTest(point) ? this : null;
        }

        private bool EllipseHitTest(Point point)
        {
            point.Offset(-datum.X, -datum.Y);
            long rx = shapeRect.Width / 2;
            long ry = shapeRect.Height / 2;
            long x = point.X - rx;
            long y = point.Y - ry;
            return x * x * ry * ry + y * y * rx * rx <= rx * rx * ry * ry;
        }
    }
}
