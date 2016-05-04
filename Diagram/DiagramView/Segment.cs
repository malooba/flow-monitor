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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram.DiagramView
{
    class Segment : Visible
    {
        public readonly Connector Connector;
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public override Cursor GetCursor => Start.X == End.X ? Cursors.SizeWE : Cursors.SizeNS;
        private readonly List<Point> points; 
        private int startIndex;
        private int endIndex;
        private bool dragHoriz;

        public Segment(Connector connector, Point start, Point end, List<Point> points)
        {
            Connector = connector;
            Start = start;
            End = end;
            this.points = points;
        }

        public bool PrepareDrag(Point grabPoint)
        {
            var dx = Math.Sign(End.X - Start.X);
            var dy = Math.Sign(End.Y - Start.Y);
            dragHoriz = dx == 0;

            var breakStart = false;
            var breakEnd = false;

            for(var i = 0; i < points.Count; i++)
            {
                if(points[i] == Start) startIndex = i;
                if(points[i] == End) endIndex = i;
            }

            if(startIndex == 0)
            {
                // Can't drag the initial two grid lengths
                if(Distance(Start, End) < View.GRID_SIZE * 2 || Distance(Start, grabPoint) < View.GRID_SIZE * 2)
                    return false;
                breakStart = true;
            }

            if(endIndex == points.Count - 1)
            {
                // Can't drag the final two grid lengths
                if(Distance(Start, End) < View.GRID_SIZE * 2 || Distance(End, grabPoint) < View.GRID_SIZE * 2)
                    return false;
                breakEnd = true;
            }

            if(breakStart)
            {
                Start = new Point(Start.X + View.GRID_SIZE * 2 * dx, Start.Y + View.GRID_SIZE * 2 * dy);
                points.Insert(1, Start);
                points.Insert(1, Start);
                startIndex = 2;
                endIndex += 2;
            }

            if(breakEnd)
            {
                End = new Point(End.X - View.GRID_SIZE * 2 * dx, End.Y - View.GRID_SIZE * 2 * dy);
                points.Insert(points.Count - 1, End);
                points.Insert(points.Count - 1, End);
                endIndex = points.Count - 3;
            }
            Debug.WriteLine("{0} points with start = {1} and end = {2}",points.Count, startIndex, endIndex);
            return true;
        }

        public Rectangle DragTo(Point d)
        {
            d = View.GridSnap(d);
            var s = points[startIndex];
            points[startIndex] = new Point(dragHoriz ? d.X : points[startIndex].X, dragHoriz ? points[startIndex].Y : d.Y);
            points[endIndex] = new Point(dragHoriz ? d.X : points[endIndex].X, dragHoriz ? points[endIndex].Y : d.Y);

            // Get invalidate rectangle;
            var rect = RubberBand.GetExtents(s, points[endIndex]);
            rect.Inflate(2, 2);
            return rect;
        }

        private static int Distance(Point s, Point t)
        {
            return (int)Math.Round(Math.Sqrt((s.X - t.X) * (s.X - t.X) + (s.Y - t.Y) * (s.Y - t.Y)));
        }
    }
}