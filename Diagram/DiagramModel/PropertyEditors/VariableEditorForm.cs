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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Diagram.DiagramModel.PropertyEditors
{
    public sealed partial class VariableEditorForm : Form
    {
        private Dictionary<string, VariableObj> variables;
        private VariableObj variable;

        private VariableObj Current
        {
            get
            {
                if(variable != null) return variable;
                return cmboVariables.SelectedItem != null ? variables[(string)cmboVariables.SelectedItem] : null;
            }
        }

        public VariableEditorForm(VariableCollection variables)
        {
            Text = "Edit Variables";
            InitializeComponent();
            this.variables = variables;
            variable = null;
            buttAdd.Visible = true;
            buttDelete.Visible = true;
            ShowVariables();
            cmboType.DataSource = DataType.AllTypes;
            VariableChanged(null, EventArgs.Empty);
        }

        private void ShowVariables()
        {
            // Don't list the workflow input variables
            foreach(var v in variables.Where(v => !v.Key.StartsWith("_")))
                cmboVariables.Items.Add(v.Key);
            if(cmboVariables.Items.Count != 0)
                cmboVariables.SelectedIndex = 0;
        }

        private void AddVariable(object sender, EventArgs e)
        {
            using(var form = new IdentifierForm<VariableObj>("Variable", variables.Keys))
            {
                if(form.ShowDialog(this) == DialogResult.OK)
                {
                    var obj = new VariableObj();
                    variables.Add(form.Identifier, obj);
                    cmboVariables.Items.Add(form.Identifier);
                    cmboVariables.SelectedIndex = cmboVariables.Items.Count - 1;
                    VariableChanged(null, EventArgs.Empty);
                }
            }
        }

        private void DeleteVariable(object sender, EventArgs e)
        {
            string name = (string)cmboVariables.SelectedItem;
            // Bad things will happen if a used variable is deleted
            var uses = Uses(name);
            if(uses.Count != 0)
            {
                MessageBox.Show(this, $"Variable {name} is in use as:\r\n{string.Join("\r\n", uses)}", "Cannot Delete");
                return;
            }
            variables.Remove(name);
            cmboVariables.Items.Clear();
            ShowVariables();
            VariableChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Find all uses of a variable
        /// </summary>
        /// <param name="variable">variable name to check</param>
        /// <returns>List of uses in readable format</returns>
        private static List<string> Uses(string variable)
        {
            var uses = new List<string>();
            foreach(var task in Model.Workflow.Tasks)
            {
                if(task.Inputs != null)
                    uses.AddRange(from input in task.Inputs 
                                  where input.Value.Var == variable 
                                  select $"Input {input.Key} to task {task.TaskId}");

                if(task.Outputs != null)
                    uses.AddRange(from output in task.Outputs 
                                  where output.Value.Var == variable 
                                  select $"Output {output.Key} from task {task.TaskId}");
            }
            return uses;
        }

        private void VariableChanged(object sender, EventArgs e)
        {
            var current = Current;
            buttDelete.Enabled = Current != null;
            cmboType.Enabled = Current != null;
            txtDescription.Enabled = Current != null;
            isInput.Enabled = Current != null;
            isLit.Enabled = Current != null;
            txtJsonPath.Enabled = Current != null;
            chkbRequired.Enabled = Current != null;
            txtDefault.Enabled = Current != null;
            if(current != null)
            {
                txtDescription.Text = current.Description;
                isInput.Checked = !string.IsNullOrWhiteSpace(current.Path);
                isLit.Checked = !isInput.Checked;
                SetInputSource();
                txtJsonPath.Text = current.Path ?? "";
                txtLit.Text = current.Lit ?? "";
                txtDefault.Text = current.Default ?? "";
                chkbRequired.Checked = current.Required;
            }
        }

        private void InputSourceChanged(object sender, EventArgs e)
        {
            SetInputSource();
        }

        /// <summary>
        /// Choose between input data or a literal value
        /// </summary>
        private void SetInputSource()
        {
            var toVar = isInput.Checked;
            txtDefault.Visible = toVar;
            chkbRequired.Visible = toVar;
            lblPath.Text = toVar ? "Json Path" : "JSON Literal";
            lblDefault.Visible = toVar;
            lblRequired.Visible = toVar;
            txtLit.Visible = !toVar;
            txtJsonPath.Visible = toVar;
            var current = Current;
            if(toVar)
            {
                current.Lit = null;
            }
            else
            {
                current.Path = null;
                current.Default = null;
            }
			txtJsonPath.Text = "";
            txtLit.Text = "";
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            if(Current != null)
                Current.Type = cmboType.Text;
        }

        private void PathChanged(object sender, EventArgs e)
        {
            Current.Path = txtJsonPath.Text;
        }

        private void RequiredChanged(object sender, EventArgs e)
        {
            Current.Required = chkbRequired.Checked;
        }

        private void DefaultChanged(object sender, EventArgs e)
        {
            Current.Default = txtDefault.Text;
        }

        private void DescriptionChanged(object sender, EventArgs e)
        {
            Current.Description = txtDescription.Text;
        }

        private void LitChanged(object sender, EventArgs e)
        {
            var current = Current;
            var dataType = DataType.FromString(current.Type);
            try
            {
                dataType.Parse(txtLit.Text);
                txtLit.ForeColor = Color.Black;
            }
            catch
            {
                txtLit.ForeColor = Color.Red;
            }
        }


        private void LitValidating(object sender, CancelEventArgs e)
        {
            var current = Current;
            var dataType = DataType.FromString(current.Type);
            try
            {
                dataType.Parse(txtLit.Text);
                current.Lit = txtLit.Text;
            }
            catch
            {
                e.Cancel = true;
            }
        }
    }
}
