using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram.DiagramView
{
    // Base class for drawing objects
    public abstract class Visible
    {
        /// <summary>
        /// Shape extent in diagram space
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        public bool Selected => View.Selection.IsSelected(this);

        internal virtual void SelectedChanged()
        { }

        public virtual Visible HitTest(Point point)
        {
            throw new NotImplementedException();
        }

        public virtual Cursor GetCursor => Cursors.Default;
    }
}
