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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Diagram.DiagramModel;

namespace Diagram.Controls.SymbolPalette
{
    public partial class SymbolPalette : UserControl
    {
        private readonly SymbolPaletteListView listView;
        private readonly ImageList shapes;

        [Browsable(true)]
        public Size ImageSize
        {
            get { return shapes.ImageSize; }
            set { shapes.ImageSize = value; }
        }
        public SymbolPalette()
        {
            InitializeComponent();

            shapes = new ImageList {TransparentColor = Color.Transparent};
            ImageSize = new Size(32, 32);

            listView = new SymbolPaletteListView
            {
                Dock = DockStyle.Fill,
                SmallImageList = shapes,
                LargeImageList = shapes
            };
            Controls.Add(listView);
        }

        public void SetImages(Dictionary<string, Image> images)
        {
            foreach(var img in images)
            {
                shapes.Images.Add(img.Key, img.Value);
            }
        }

        public void Add(PaletteObj symbol)
        {
            listView.Add(symbol);
        }
    }
}
