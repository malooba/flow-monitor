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
    class SelectionFrame
    {
        private readonly Color background;
        private Point origin;
        private Rectangle currentFrame;
        public Rectangle Frame => currentFrame;

        public SelectionFrame(Point origin, Color background)
        {
            this.origin = origin;
            this.background = background;
            currentFrame = Rectangle.Empty;
        }

        public void UpdateFrame(Point dragged)
        {
            var newFrame = new Rectangle(
               Math.Min(origin.X, dragged.X),
               Math.Min(origin.Y, dragged.Y),
               Math.Abs(origin.X - dragged.X),
               Math.Abs(origin.Y - dragged.Y));

            if(newFrame == currentFrame)
                return;

            if(currentFrame != Rectangle.Empty)
                ControlPaint.DrawReversibleFrame(currentFrame, background, FrameStyle.Thick);
            ControlPaint.DrawReversibleFrame(newFrame, background, FrameStyle.Thick);
            currentFrame = newFrame;
        }
    }
}
