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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Diagram.DiagramModel;
using Diagram.DiagramView;
using View = Diagram.DiagramView.View;

namespace Diagram.Controls.DiagramControl
{
    public sealed partial class Document : UserControl
    {
        private View view;
        private Shape draggedShape;
        private Segment draggedSegment;
        private Point segmentGrabPoint;
        private bool dragStarted;
        private Point shapeInitialLocation;
        private Point dragPoint;
        private RubberBand rubberBand;
        private SelectionFrame selectionFrame;
        // The additional space around the document to allow for growth
        private const int DOCUMENT_MARGIN = 100;
        private bool panning;
        private Point panClick;
        private int minLeft;
        private int minTop;

        public bool ReadOnly
        { get; set; }

        private Point ScrollPosition 
        {
            get { return ((Panel)Parent).AutoScrollPosition; }
            set { ((Panel)Parent).AutoScrollPosition = value; }
        }

        public float Zoom
        {
            get { return zoom; }

            set
            {
                if(value < 2f && value > 0.25f)
                {
                    zoom = value;
                    zoomMatrix = new Matrix(zoom, 0f, 0f, zoom, 0f, 0f);
                }
            }
        }

        private float zoom;
        private Matrix zoomMatrix;

        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadCursorFromFile(string str);


        public Document()
        {
            InitializeComponent();
            Cursor = Cursors.Default;
            AllowDrop = true;
			BackColor = SystemColors.Control;
            Zoom = 1f;
        }

        public View View
        {
            get { return view; }
            set
            {
                view = value;
                if(view != null)
                {
                    view.GraphicsChanged += UpdateGraphics;
                    UpdateSize();
                }
            }
        }

        private void UpdateGraphics(object sender, UpdateGraphicsEventArgs e)
        {
            Invalidate();
        }

		private void UpdateSize()
		{
			var bounds = view.Model.GetDiagramBounds(0);
            Size = new Size(bounds.Right + DOCUMENT_MARGIN, bounds.Bottom + DOCUMENT_MARGIN);
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Transform = zoomMatrix;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.FillRectangle(new SolidBrush(BackColor), e.ClipRectangle);
            view?.Render(g);
            rubberBand?.Render(g);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var mouse = MouseZoom(e.Location);
            var obj = view.HitTest(mouse);
            if(obj is Shape)
            {
                if(ModifierKeys == Keys.Control)
                {
                    Debug.WriteLine("toggling object");
                    View.Selection.Toggle(obj);
                    Invalidate();
                }
                else if(ModifierKeys == Keys.None)
                {
                    View.Selection.Select(obj);
                    if(!ReadOnly)
                        StartShapeDrag((Shape)obj, mouse);
                    Invalidate();
                }
                
            }
            else if(obj is Pin && !ReadOnly && ModifierKeys == Keys.None)
            {
                rubberBand = new RubberBand(this, view, obj as Pin);
            }
            else if(obj is Segment && !ReadOnly)
            {
                var connector = (obj as Segment).Connector;
                if(ModifierKeys == Keys.Control)
                {
                    View.Selection.Toggle(connector);
                    Invalidate();
                }
                else if(ModifierKeys == Keys.None)
                {
                    View.Selection.Select(connector);
                    StartSegmentDrag(mouse, (obj as Segment));
                    Invalidate();
                }
            }
            else
            {
                // User clicked on background
                if(ModifierKeys == Keys.Control)
                {
                    selectionFrame = new SelectionFrame(PointToScreen(e.Location), BackColor);
                }
                else if(ModifierKeys == Keys.None)
                {
                    if(View.Selection.Count != 0)
                    {
                        Invalidate();
                        View.Selection.Clear();
                    }
                    panning = true;
                    panClick = mouse;
                    minLeft = Math.Min(0, Parent.Width - Width);
                    minTop = Math.Min(0, Parent.Height - Height);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var mouse = MouseZoom(e.Location);
            if(draggedShape != null)
            {
                DragShape(e);
                if(!dragStarted && shapeInitialLocation != draggedShape.Location)
                {
                    foreach(var conn in Connector.GetConnections(draggedShape.Task))
                        conn.UnRoute();
                    dragStarted = true;
                    Invalidate();
                }
            }
            else if(rubberBand != null)
            {
                rubberBand.MouseMove(mouse);
            }
            else if(draggedSegment != null)
            {
                if(!dragStarted)
                {
                    if(draggedSegment.PrepareDrag(segmentGrabPoint))
                    {
                        dragStarted = true;
                    }
                    else
                    {
                        draggedSegment = null;
                        return;
                    }
                }
                Invalidate(draggedSegment.DragTo(mouse));
            }
            else if(panning)
            {
                Cursor = Cursors.SizeAll;    // TODO: Get the correct Grabby Hand cursor

                // If the diagram has shrunk then permit scrolling to the new extended limits
                // (minLeft, minTop are the normal scroll limits for a diagram that has not just been shrunk)
                var llimit = Math.Min(Left, minLeft);
                var tlimit = Math.Min(Top, minTop);
                // Scroll the diagram 
                var left = Math.Min(0, Math.Max(llimit, Left + e.X - panClick.X));
                var top = Math.Min(0, Math.Max(tlimit, Top + e.Y - panClick.Y));
                Location = new Point(left, top);
            }
            else if(selectionFrame != null)
            {
                selectionFrame.UpdateFrame(PointToScreen(e.Location));
            }
            else
            {
                var obj = view.HitTest(mouse);
                if(!ReadOnly && obj != null)
                    Cursor.Current = obj.GetCursor;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var mouse = MouseZoom(e.Location);
            if(draggedShape != null)
            {
                DragShape(e);

                if(dragStarted)
                    view.Model.ReRouteTask(draggedShape.Task);

                Invalidate();
                draggedShape = null;
            }
            if(draggedSegment != null)
            {
                draggedSegment.Connector.Normalise();
                draggedSegment = null;
            }
            if(rubberBand != null)
            {
                rubberBand.MouseUp(mouse);
                rubberBand = null;
                Invalidate();
            }
            if(panning)
            {
                Cursor = Cursors.Default;
                panning = false;
                Invalidate();
            }
            if(selectionFrame != null)
            {
                var frame = selectionFrame.Frame;
                var topLeft = PointToClient(frame.Location);
                var rect = new Rectangle(
                    (int)Math.Floor(topLeft.X / zoom),
                    (int)Math.Floor(topLeft.Y / zoom),
                    (int)Math.Ceiling(frame.Width / zoom),
                    (int)Math.Ceiling(frame.Height/zoom));
                View.Selection.SelectInRectangle(rect);
                selectionFrame = null;
                Invalidate();
            }
			UpdateSize();
        }

        private void DragShape(MouseEventArgs e)
        {
            var mouse = MouseZoom(e.Location);
            var rect = draggedShape.Bounds;

            var p = mouse;
            // Translate to shape datum
            p.Offset(-dragPoint.X, -dragPoint.Y);
            // Quantise to grid
            var snapped = View.GridSnap(p);

            draggedShape.Location = draggedShape.LocationFromDatum(snapped);
            rect = Rectangle.Union(rect, draggedShape.Bounds);
            Invalidate(rect);
        }

        private void StartShapeDrag(Shape shape, Point p)
        {
            draggedShape = shape;
            shapeInitialLocation = shape.Location;
            dragPoint = shape.DiagramToDatum(p);
            dragStarted = false;
        }

        private void StartSegmentDrag(Point grabPoint, Segment segment)
        {
            draggedSegment = segment;
            dragStarted = false;
            segmentGrabPoint = grabPoint;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = ReadOnly ? DragDropEffects.None : e.AllowedEffect;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            e.Effect = DragDropEffects.None;
            if(ReadOnly) return;

            var dragged = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;

            if(dragged?.Tag is PaletteObj) 
                e.Effect = DragDropEffects.Copy;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            var mouse = MouseZoom(new Point(e.X, e.Y));
            var dragged = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if(!(dragged?.Tag is PaletteObj)) 
                return;

            var symbolType = (PaletteObj)dragged.Tag;

            var task = view.CreateTask(symbolType, View.GridSnap(PointToClient(mouse)));

            if(task != null)
            {
                UpdateSize();
                // Re-route any connections that we dropped it on
                view.Model.ReRouteTask(task);
            }

            Invalidate();
        }

        public void DeleteSelected()
        {
            view.DeleteSelected();
        }

        private Point MouseZoom(Point p)
        {
            return  new Point((int)Math.Round(p.X / zoom), (int)Math.Round(p.Y / zoom));
        }
    }
}
