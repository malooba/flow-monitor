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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diagram.DiagramModel.PropertyEditors
{
    public partial class TaskInputEditorForm : Form
    {
        private InputCollection inputs;

        /// <summary>
        /// The current Input object is either the single input being edited or one of the Input collection items
        /// (The input collection may be empty
        /// </summary>
		private InputObj Current => (string)cmboInputs.SelectedItem == null ? null : inputs[(string)cmboInputs.SelectedItem];

        public TaskInputEditorForm(InputCollection inputs)
        {
			Text = "Task Inputs";
            InitializeComponent();
            cmboType.DataSource = DataType.AllTypes;
            this.inputs = inputs ?? new InputCollection();
            foreach(var i in inputs.Where(i => !i.Value.Hidden))
                cmboInputs.Items.Add(i.Key);
            if(Model.Workflow.Variables != null)
                foreach(var variable in Model.Workflow.Variables)
                    cmboVariables.Items.Add(variable);

            ResetInputCombo();
            InputChanged(null, EventArgs.Empty); 
        }

        private void AddInput(object sender, EventArgs e)
        {
            using(var form = new IdentifierForm<InputObj>("Input", inputs.Keys))
            {
                if(form.ShowDialog(this) == DialogResult.OK)
                {
                    var obj = new InputObj
                    {
                        UserDefined = true,
                        Type = "any"
                    };
                    inputs.Add(form.Identifier, obj);
                    cmboInputs.Items.Add(form.Identifier);
                    cmboInputs.SelectedIndex = cmboInputs.FindStringExact(form.Identifier);
                    InputChanged(null, EventArgs.Empty);
                }
            }
        }

        private void DeleteInput(object sender, EventArgs e)
        {
            inputs.Remove((string)cmboInputs.SelectedItem);
            cmboInputs.Items.Clear();
            foreach(var i in inputs)
                cmboInputs.Items.Add(i.Key);
            ResetInputCombo();
            InputChanged(null, EventArgs.Empty);
            
        }

        private void ResetInputCombo()
        {
            if(cmboInputs.Items.Count > 0)
                cmboInputs.SelectedIndex = 0;
        }

        private void InputChanged(object sender, EventArgs e)
        {
			var current = Current;
            EnableEditControls();
            isVar.Checked = !string.IsNullOrWhiteSpace(current?.Var);
            isLit.Checked = !isVar.Checked;
            SetInputSource();
            if(current == null)
            {
                txtLit.Text = "";
                txtDefault.Text = "";
                txtDescription.Text = "";
                txtJsonPath.Text = "";
                cmboVariables.SelectedIndex = -1;
                return;  
            }

            // Set this first because changes null out Lit and Default
            cmboType.SelectedIndex = cmboType.SelectedIndex = cmboType.FindStringExact(current.Type ?? "any");
            txtDescription.Text = current.Description;
            txtLit.Text =  (current.Lit ?? "").ToString(Formatting.None);
            cmboVariables.SelectedIndex = cmboVariables.FindStringExact(current.Var);
            
            txtJsonPath.Text = current.Path;
            txtDefault.Text = (current.Default ?? "").ToString();
            chkbRequired.Checked = current.Required;
        }

        private void EnableEditControls()
        {
            var current = Current;
            var enable = current != null;
            var userDefined = current?.UserDefined ?? false;
            // only user defined inputs can be deleted or have their description, type or required status changed
            cmboType.Enabled = userDefined;
            buttDelete.Enabled = userDefined;
            chkbRequired.Enabled = userDefined; 
            txtDescription.Enabled = enable;
            txtDescription.ReadOnly = !userDefined; 
            cmboType.Enabled = enable;
            buttDelete.Enabled = enable;
            chkbRequired.Enabled = enable;
            isVar.Enabled = enable;
            isLit.Enabled = enable;
            cmboVariables.Enabled = enable;
            txtJsonPath.Enabled = enable;
            txtDefault.Enabled = enable;
            txtLit.Enabled = enable;
        }


        private void InputSourceChanged(object sender, EventArgs e)
        {
            SetInputSource();
        }

        private void SetInputSource()
        {
            var toVar = isVar.Checked;
            cmboVariables.Visible = toVar;
            txtLit.Visible = !toVar;
            buttEditor.Visible = !toVar;
            txtJsonPath.Visible = toVar;
            txtDefault.Visible = toVar;
            chkbRequired.Visible = toVar;
            lblPath.Visible = toVar;
            lblDefault.Visible = toVar;
            lblRequired.Visible = toVar;
            lblValue.Text = toVar ? "Variable Name" : "Literal JSON";
            var current = Current;
            if (current == null) return;
            if(toVar)
            {
                current.Lit = null;
                txtLit.Text = null;
            }
            else
            {
                current.Var = null;
                cmboVariables.SelectedItem = null;
                current.Path = null;
                txtJsonPath.Text = null;
                current.Default = null;
                txtDefault.Text = null;
            }
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
            var dataType = DataType.FromString(Current.Type);
            try
            {
                dataType.Parse(txtLit.Text);
                Current.Default = JToken.Parse(txtDefault.Text);
                txtDefault.ForeColor = Color.Black;
            }
            catch(Exception)
            {
                txtDefault.ForeColor = Color.Red;
            }
            
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
                current.Lit = JToken.Parse(txtLit.Text);
            }
            catch
            {
                e.Cancel = true;
            }
        }

        private void VarChanged(object sender, EventArgs e)
        {
			Current.Var = cmboVariables.Text;
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            var current = Current;

            if(current != null && current.Type != cmboType.Text)
            {
                current.Type = cmboType.Text;
                current.Lit = null;
                current.Default = null;
            }
        }

        private void DescriptionChanged(object sender, EventArgs e)
        {
            Current.Description = txtDescription.Text;
        }

        private void OpenLiteralEditor(object sender, EventArgs e)
        {
            var current = Current;
            using(var editor = new TextEditor(current.Type, current.Lit))
            {
                if(editor.ShowDialog() == DialogResult.OK)
                {
                    // Convert the 
                    txtLit.Text = editor.Result.ToString(Formatting.None);
                    current.Lit = editor.Result;
                }
            }
        }
    }
}
