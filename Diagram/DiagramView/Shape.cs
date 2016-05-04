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
using System.Windows.Forms;
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    /// <summary>
    /// Base of symbol shape classes
    /// </summary>
    public abstract class Shape : Visible
    {
        public readonly TaskObj Task;
        protected const int Margin = 8;
        protected Size DefaultSize;
        protected static readonly Font Font = new Font("Lucida Console", 9.0f);
        
        protected Size size;
        protected Rectangle idRect;
        protected Rectangle shapeRect;
        protected readonly SolidBrush brush = new SolidBrush(Color.LightBlue);
        protected readonly SolidBrush selectedBrush = new SolidBrush(Color.SteelBlue);
        protected readonly SolidBrush backBrush = new SolidBrush(SystemColors.Window);
        internal Dictionary<string, Pin> pins;
        protected readonly Pen pen = new Pen(Color.Black, 2f);

        /// <summary>
        /// The top left of the symbol shape. All shape features (corners, centre, pins etc) are located on a 10x10 grid relative to the datum
        /// The datum is snapped to the diagram grid to ensure that features on any two shapes line up.
        /// </summary>
        protected Point datum;

        // Datum location in diagram coordinates
        public Point DatumLocation => new Point(Bounds.X + datum.X, Bounds.Y + datum.Y);
        protected Image image;
        protected int fontHeight;
        private bool needRedraw = true;

        protected readonly StringFormat labelFormat = new StringFormat
        {
            FormatFlags = StringFormatFlags.LineLimit,
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };

        protected readonly StringFormat idFormat = new StringFormat
        {
            FormatFlags = StringFormatFlags.LineLimit,
            LineAlignment = StringAlignment.Near,
            Alignment = StringAlignment.Near,
            Trimming = StringTrimming.EllipsisCharacter
        };

        public static Shape Create(string style, TaskObj task, Point datumLocation)
        {
            switch(style.ToLower())
            {
                case "circle":
                    return new CircleShape(task, datumLocation);

                case "box":
                    return new BoxShape(task, datumLocation);

                default:
                    throw new ApplicationException("Unknown shape style: " + style);
            }
        }

        protected Shape(TaskObj task)
        {
            Task = task;
        }

        /// <summary>
        /// Initialise the shape
        /// Only call this from the constructor of sealed classes as it uses virtual methods and properties
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="datumLocation"></param>
        protected void InitShape(int width, int height, Point datumLocation)
        {
            size = new Size(width, height);
            CalculateBounds();
            Location = LocationFromDatum(datumLocation);
            AddPins();
            image = new Bitmap(Bounds.Width, Bounds.Height);
            
        }

        /// <summary>
        /// Location of top right in diagram space
        /// </summary>
        public Point Location
        {
            get { return Bounds.Location; }
            set { Bounds = new Rectangle(value, Bounds.Size); }
        }

        /// <summary>
        /// The interior colour of the shape
        /// </summary>
        public Color FillColour
        {
            get { return brush.Color; }
            set
            {
                brush.Color = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The background colour in the bounds rectangle
        /// This should match the colour of the drawing surface
        /// </summary>
        public Color BackColour
        {
            get { return backBrush.Color; }
            set
            {
                backBrush.Color = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Invalidate the cached image of this shape
        /// </summary>
        public void Invalidate()
        {
            needRedraw = true;
        }

        /// <summary>
        /// The mouse-over cursor to use
        /// </summary>
        public override Cursor GetCursor => Cursors.Hand;

        /// <summary>
        /// Render shape to a Graphics surface
        /// </summary>
        /// <param name="g"></param>
        public void Render(Graphics g)
        {
            if(needRedraw)
            {
                Redraw();
                needRedraw = false;
            }
            g.DrawImageUnscaled(image, Bounds);
        }

        /// <summary>
        /// Calculate the shape bounds
        /// Also calculate other shape geometry values along the way
        /// </summary>
        protected void CalculateBounds()
        {
            var g = Graphics.FromImage(new Bitmap(1, 1));
            var m = g.GetFontMetrics(Font);
            fontHeight = m.Height - m.InternalLeading;
            // This will need to be adjusted if the pins are made larger (unlikely)
            var extra = 3;
            datum = new Point(Margin, fontHeight + extra);
            Bounds = new Rectangle(new Point(Location.X, Location.Y), new Size(size.Width + Margin * 2, size.Height + Margin + fontHeight + extra));
            shapeRect = new Rectangle(datum, size);
            idRect = new Rectangle
            {
                X = datum.X + 5,
                Y = 0,
                Height = fontHeight,
                Width = size.Width - 10
            };
        }

        /// <summary>
        /// Add the connection pins specified by the task
        /// </summary>
        protected abstract void AddPins();

        /// <summary>
        /// Redraw the shape into the image cache
        /// </summary>
        protected abstract void Redraw();

        /// <summary>
        /// Get the graphics surface for the image cache
        /// </summary>
        /// <returns></returns>
        protected Graphics GetImageGraphics()
        {
            var g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.Transparent);
            return g;
        }

        internal override void SelectedChanged()
        {
            Redraw();
        }

        /// <summary>
        /// Transform a diagram referred point to a datum referred point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point DiagramToDatum(Point p)
        {
            return new Point
            {
                X = p.X - Bounds.X - datum.X,
                Y = p.Y - Bounds.Y - datum.Y
            };
        }

        /// <summary>
        /// Return the Location to place the shape datum at the diagram referred point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point LocationFromDatum(Point p)
        {
            return new Point
            {
                X = p.X - datum.X,
                Y = p.Y - datum.Y
            };
        }

        public string FindPin(Pin pin)
        {
            return pins.Single(p => p.Value == pin).Key;
        }
    }
}
