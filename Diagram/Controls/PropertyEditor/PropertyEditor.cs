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
using Diagram.DiagramModel;

namespace Diagram.Controls.PropertyEditor
{
    /// <summary>
    /// The right hand pane of the workflow graph editor
    /// </summary>
    public partial class PropertyEditor : UserControl
    {
        public WorkflowObj Workflow
        {
            get { return workflow; }
            set
            {
                workflow = value;
                SetSelection(workflow);
            }
        }
        private WorkflowObj workflow;
   
        public PropertyEditor()
        {
            InitializeComponent();
        }

        public bool ReadOnly
        {
            get { return propertyGrid1.ReadOnly;  }
            set { propertyGrid1.ReadOnly = value; }
        }

        public void SetSelection(object obj)
        {
            propertyGrid1.SelectedObject = obj;
        }
    }
}
