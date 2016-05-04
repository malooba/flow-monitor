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

using System.Windows.Forms;
using View = Diagram.DiagramView.View;

namespace Diagram.Controls.DiagramControl
{
    /// <summary>
    /// Control to display and edit workflow graphs
    /// </summary>
    public sealed partial class DiagramControl : UserControl
    {
        public Document Document => document;

        public DiagramControl(View view)
        {
            InitializeComponent();
            document.View = view;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }
    }
}
