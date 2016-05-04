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
using System.IO;
using System.Windows.Forms;
using Diagram.DiagramModel;
using Diagram.PaletteManager;
using Newtonsoft.Json;

namespace FlowMonitor.ViewModules.Workflows
{
    public partial class WorkflowEditor : UserControl
    {
        public event EventHandler SaveWorkflow;
        public event EventHandler DiscardWorkflow;

        public WorkflowEditor()
        {
            InitializeComponent();
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var reader = new JsonTextReader(new StreamReader(Path.Combine(path, "palette.json")));
            var paletteItems = new JsonSerializer().Deserialize<List<PaletteObj>>(reader);
            new PaletteManager(symbolPalette1, paletteItems);
        }

        private void Save(object sender, EventArgs e)
        {
            SaveWorkflow?.Invoke(this, EventArgs.Empty);
        }

        private void Discard(object sender, EventArgs e)
        {
            if(MessageBox.Show(this, "Are you sure you want to discard this edit?", "Discard Edit",  MessageBoxButtons.YesNo) == DialogResult.Yes)
                DiscardWorkflow?.Invoke(this, EventArgs.Empty);
        }
    }
}
