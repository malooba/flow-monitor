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
using System.Runtime.InteropServices;

namespace Diagram.DiagramView
{
    static class GraphicsExtension
    {
        public static FontMetrics GetFontMetrics(this Graphics graphics, Font font)
        {
            return FontMetricsImpl.GetFontMetrics(graphics, font);
        }

        private class FontMetricsImpl : FontMetrics
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct TEXTMETRIC
            {
                public int tmHeight;
                public int tmAscent;
                public int tmDescent;
                public int tmInternalLeading;
                public int tmExternalLeading;
                public int tmAveCharWidth;
                public int tmMaxCharWidth;
                public int tmWeight;
                public int tmOverhang;
                public int tmDigitizedAspectX;
                public int tmDigitizedAspectY;
                public char tmFirstChar;
                public char tmLastChar;
                public char tmDefaultChar;
                public char tmBreakChar;
                public byte tmItalic;
                public byte tmUnderlined;
                public byte tmStruckOut;
                public byte tmPitchAndFamily;
                public byte tmCharSet;
            }

            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern bool GetTextMetrics(IntPtr hdc, ref TEXTMETRIC lptm);

            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern bool DeleteObject(IntPtr hdc);

            private TEXTMETRIC GenerateTextMetrics(Graphics graphics, Font font)
            {
                var hDC = IntPtr.Zero;
                TEXTMETRIC textMetric = new TEXTMETRIC();
                var hFont = IntPtr.Zero;
                try
                {
                    hDC = graphics.GetHdc();
                    hFont = font.ToHfont();
                    IntPtr hFontDefault = SelectObject(hDC, hFont);
                    bool result = GetTextMetrics(hDC, ref textMetric);
                    SelectObject(hDC, hFontDefault);
                }
                finally
                {
                    if(hFont != IntPtr.Zero) DeleteObject(hFont);
                    if(hDC != IntPtr.Zero) graphics.ReleaseHdc(hDC);
                }
                return textMetric;
            }

            private TEXTMETRIC metrics;
            public override int Height => metrics.tmHeight;
            public override int Ascent => metrics.tmAscent;
            public override int Descent => metrics.tmDescent;
            public override int InternalLeading => metrics.tmInternalLeading;
            public override int ExternalLeading => metrics.tmExternalLeading;
            public override int AverageCharacterWidth => metrics.tmAveCharWidth;
            public override int MaximumCharacterWidth => metrics.tmMaxCharWidth;
            public override int Weight => metrics.tmWeight;
            public override int Overhang => metrics.tmOverhang;
            public override int DigitizedAspectX => metrics.tmDigitizedAspectX;
            public override int DigitizedAspectY => metrics.tmDigitizedAspectY;

            private FontMetricsImpl(Graphics graphics, Font font)
            {
                metrics = GenerateTextMetrics(graphics, font);
            }

            public static FontMetrics GetFontMetrics(Graphics graphics, Font font)
            {
                return new FontMetricsImpl(graphics, font);
            }
        }
    }

    public abstract class FontMetrics
    {
        public virtual int Height => 0;
        public virtual int Ascent => 0;
        public virtual int Descent => 0;
        public virtual int InternalLeading => 0;
        public virtual int ExternalLeading => 0;
        public virtual int AverageCharacterWidth => 0;
        public virtual int MaximumCharacterWidth => 0;
        public virtual int Weight => 0;
        public virtual int Overhang => 0;
        public virtual int DigitizedAspectX => 0;
        public virtual int DigitizedAspectY => 0;
    }
}
