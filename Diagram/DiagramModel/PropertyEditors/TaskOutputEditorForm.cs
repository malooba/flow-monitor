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
using System.Linq;
using System.Windows.Forms;

namespace Diagram.DiagramModel.PropertyEditors
{
    public partial class TaskOutputEditorForm : Form
    {
        private readonly Dictionary<string, OutputObj> outputs;
        private bool updating;

		private OutputObj Current => (string)cmboOutputs.SelectedItem == null ? null : outputs[(string)cmboOutputs.SelectedItem];

        public TaskOutputEditorForm(OutputCollection outputs)
        {
			Text = "Task Outputs";
            InitializeComponent();
            this.outputs = outputs ?? new OutputCollection();
			buttAdd.Visible = true;
			buttDelete.Visible = true;
			if(outputs != null)
				foreach(var o in outputs)
					cmboOutputs.Items.Add(o.Key);
            cmboVariables.Items.Add(new KeyValuePair<string, VariableObj>("(none)", null));
            if(Model.Workflow.Variables != null)
				foreach(var variable in Model.Workflow.Variables.Where(v => !v.Key.StartsWith("_")))
					cmboVariables.Items.Add(variable);
            cmboType.DataSource = DataType.AllTypes;
            if(cmboOutputs.Items.Count > 0)
                cmboOutputs.SelectedIndex = 0;
               
            OutputChanged(null, EventArgs.Empty);
        }

        private void EnableEditControls()
        {
            var current = Current;
            var enable = current != null;
            var userDefined = current?.UserDefined ?? false;
            cmboOutputs.Enabled = enable;
            cmboVariables.Enabled = enable;
            cmboType.Enabled = userDefined;
            buttDelete.Enabled = userDefined;
            txtDescription.Enabled = enable;
            txtDescription.ReadOnly = !userDefined;
        }

        private void OutputChanged(object sender, EventArgs e)
        {
            updating = true;
            var current = Current;
            EnableEditControls();
            
            txtDescription.Text = current?.Description;
            cmboType.SelectedIndex = cmboType.FindStringExact(current?.Type ?? "any");
            cmboVariables.SelectedIndex = current == null ? -1 : cmboVariables.FindStringExact(current.Var);
            updating = false;
        }

        private void AddOutput(object sender, EventArgs e)
        {
            using(var form = new IdentifierForm<OutputObj>("Output", outputs.Keys))
            {
                if(form.ShowDialog(this) == DialogResult.OK)
                {
                    var obj = new OutputObj
                    {
                        UserDefined = true,
                        Type = "any"
                    };
                    outputs.Add(form.Identifier, obj);
                    cmboOutputs.Items.Add(form.Identifier);
                    cmboOutputs.SelectedIndex = cmboOutputs.FindStringExact(form.Identifier);
                    cmboVariables.SelectedIndex = -1;
                    OutputChanged(null, EventArgs.Empty);
                }
            }
        }

        private void DeleteOutput(object sender, EventArgs e)
        {
            outputs.Remove((string)cmboOutputs.SelectedItem);
            cmboOutputs.Items.Clear();
            foreach(var input in outputs)
                cmboOutputs.Items.Add(input.Key);
            if(outputs.Count != 0)
                cmboOutputs.SelectedIndex = 0;
            OutputChanged(null, EventArgs.Empty);
        }

        private void UpdateOutput(object sender, EventArgs e)
        {
            if(updating || cmboVariables.SelectedItem == null) return;
            var variable = ((KeyValuePair<string, VariableObj>)cmboVariables.SelectedItem).Key;
            if(variable == "(none)") variable = null;
            var type = cmboType.SelectedItem.ToString();
            Current.Var = variable;
            Current.Type = type;
            Current.Description = txtDescription.Text;
        }
    }
}
