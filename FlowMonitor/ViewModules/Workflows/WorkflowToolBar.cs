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
using System.Windows.Forms;

namespace FlowMonitor.ViewModules.Workflows
{
    public partial class WorkflowToolBar : UserControl
    {
        public event EventHandler Edit;
        public event EventHandler DeleteSelected;
        public event EventHandler Save;

        public WorkflowToolBar()
        {
            InitializeComponent();
        }

        public bool EnableEditing
        {
            get { return tbutDeleteSelected.Enabled; }
            set { tbutDeleteSelected.Enabled = value; }
        }

        // <summary>
        // Relay clicks on butons
        // </summary>
        private void DeleteSelectedClicked(object sender, EventArgs e)
        {
            DeleteSelected?.Invoke(this, EventArgs.Empty);
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            Save?.Invoke(this, EventArgs.Empty);
        }

        private void EditClicked(object sender, EventArgs e)
        {
            Edit?.Invoke(this, EventArgs.Empty);
        }
    }
}
