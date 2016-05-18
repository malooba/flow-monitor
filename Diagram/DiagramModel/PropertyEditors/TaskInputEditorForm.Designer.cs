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

namespace Diagram.DiagramModel.PropertyEditors
{
    partial class TaskInputEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblValue = new System.Windows.Forms.Label();
            this.txtLit = new System.Windows.Forms.TextBox();
            this.txtJsonPath = new JsonPathInput();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtDefault = new System.Windows.Forms.TextBox();
            this.lblDefault = new System.Windows.Forms.Label();
            this.chkbRequired = new System.Windows.Forms.CheckBox();
            this.isVar = new System.Windows.Forms.RadioButton();
            this.isLit = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.buttOk = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.cmboInputs = new System.Windows.Forms.ComboBox();
            this.buttDelete = new System.Windows.Forms.Button();
            this.buttAdd = new System.Windows.Forms.Button();
            this.cmboVariables = new System.Windows.Forms.ComboBox();
            this.cmboType = new System.Windows.Forms.ComboBox();
            this.buttEditor = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(13, 195);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(76, 13);
            this.lblValue.TabIndex = 0;
            this.lblValue.Text = "Variable Name";
            // 
            // txtLit
            // 
            this.txtLit.Location = new System.Drawing.Point(95, 192);
            this.txtLit.Multiline = true;
            this.txtLit.Name = "txtLit";
            this.txtLit.Size = new System.Drawing.Size(153, 92);
            this.txtLit.TabIndex = 1;
            this.txtLit.TextChanged += new System.EventHandler(this.LitChanged);
            this.txtLit.Validating += new System.ComponentModel.CancelEventHandler(this.LitValidating);
            // 
            // txtJsonPath
            // 
            this.txtJsonPath.Location = new System.Drawing.Point(95, 218);
            this.txtJsonPath.Name = "txtJsonPath";
            this.txtJsonPath.Size = new System.Drawing.Size(178, 20);
            this.txtJsonPath.TabIndex = 3;
            this.txtJsonPath.TextChanged += new System.EventHandler(this.PathChanged);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(13, 221);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(60, 13);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "JSON Path";
            // 
            // txtDefault
            // 
            this.txtDefault.Location = new System.Drawing.Point(95, 264);
            this.txtDefault.Name = "txtDefault";
            this.txtDefault.Size = new System.Drawing.Size(178, 20);
            this.txtDefault.TabIndex = 5;
            this.txtDefault.TextChanged += new System.EventHandler(this.DefaultChanged);
            // 
            // lblDefault
            // 
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new System.Drawing.Point(13, 267);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(41, 13);
            this.lblDefault.TabIndex = 4;
            this.lblDefault.Text = "Default";
            // 
            // chkbRequired
            // 
            this.chkbRequired.AutoSize = true;
            this.chkbRequired.Location = new System.Drawing.Point(95, 244);
            this.chkbRequired.Name = "chkbRequired";
            this.chkbRequired.Size = new System.Drawing.Size(15, 14);
            this.chkbRequired.TabIndex = 7;
            this.chkbRequired.UseVisualStyleBackColor = true;
            this.chkbRequired.CheckedChanged += new System.EventHandler(this.RequiredChanged);
            // 
            // isVar
            // 
            this.isVar.AutoSize = true;
            this.isVar.Checked = true;
            this.isVar.Location = new System.Drawing.Point(15, 169);
            this.isVar.Name = "isVar";
            this.isVar.Size = new System.Drawing.Size(63, 17);
            this.isVar.TabIndex = 8;
            this.isVar.TabStop = true;
            this.isVar.Text = "Variable";
            this.isVar.UseVisualStyleBackColor = true;
            this.isVar.CheckedChanged += new System.EventHandler(this.InputSourceChanged);
            // 
            // isLit
            // 
            this.isLit.AutoSize = true;
            this.isLit.Location = new System.Drawing.Point(95, 169);
            this.isLit.Name = "isLit";
            this.isLit.Size = new System.Drawing.Size(53, 17);
            this.isLit.TabIndex = 9;
            this.isLit.Text = "Literal";
            this.isLit.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Input:";
            // 
            // buttOk
            // 
            this.buttOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttOk.Location = new System.Drawing.Point(116, 298);
            this.buttOk.Name = "buttOk";
            this.buttOk.Size = new System.Drawing.Size(75, 23);
            this.buttOk.TabIndex = 11;
            this.buttOk.Text = "OK";
            this.buttOk.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Type: ";
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(13, 244);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 16;
            this.lblRequired.Text = "Required";
            // 
            // cmboInputs
            // 
            this.cmboInputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboInputs.FormattingEnabled = true;
            this.cmboInputs.Location = new System.Drawing.Point(59, 11);
            this.cmboInputs.Name = "cmboInputs";
            this.cmboInputs.Size = new System.Drawing.Size(158, 21);
            this.cmboInputs.TabIndex = 17;
            this.cmboInputs.SelectionChangeCommitted += new System.EventHandler(this.InputChanged);
            // 
            // buttDelete
            // 
            this.buttDelete.Location = new System.Drawing.Point(223, 38);
            this.buttDelete.Name = "buttDelete";
            this.buttDelete.Size = new System.Drawing.Size(49, 23);
            this.buttDelete.TabIndex = 19;
            this.buttDelete.Text = "Delete";
            this.buttDelete.UseVisualStyleBackColor = true;
            this.buttDelete.Click += new System.EventHandler(this.DeleteInput);
            // 
            // buttAdd
            // 
            this.buttAdd.Location = new System.Drawing.Point(223, 9);
            this.buttAdd.Name = "buttAdd";
            this.buttAdd.Size = new System.Drawing.Size(48, 23);
            this.buttAdd.TabIndex = 20;
            this.buttAdd.Text = "Add";
            this.buttAdd.UseVisualStyleBackColor = true;
            this.buttAdd.Click += new System.EventHandler(this.AddInput);
            // 
            // cmboVariables
            // 
            this.cmboVariables.DisplayMember = "Key";
            this.cmboVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboVariables.FormattingEnabled = true;
            this.cmboVariables.Location = new System.Drawing.Point(94, 192);
            this.cmboVariables.Name = "cmboVariables";
            this.cmboVariables.Size = new System.Drawing.Size(178, 21);
            this.cmboVariables.TabIndex = 22;
            this.cmboVariables.SelectedValueChanged += new System.EventHandler(this.VarChanged);
            // 
            // cmboType
            // 
            this.cmboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboType.FormattingEnabled = true;
            this.cmboType.Location = new System.Drawing.Point(59, 40);
            this.cmboType.Name = "cmboType";
            this.cmboType.Size = new System.Drawing.Size(158, 21);
            this.cmboType.TabIndex = 23;
            this.cmboType.SelectedValueChanged += new System.EventHandler(this.TypeChanged);
            // 
            // buttEditor
            // 
            this.buttEditor.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttEditor.Location = new System.Drawing.Point(254, 191);
            this.buttEditor.Name = "buttEditor";
            this.buttEditor.Size = new System.Drawing.Size(18, 23);
            this.buttEditor.TabIndex = 24;
            this.buttEditor.Text = "…";
            this.buttEditor.UseVisualStyleBackColor = true;
            this.buttEditor.Click += new System.EventHandler(this.OpenLiteralEditor);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(15, 96);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(259, 62);
            this.txtDescription.TabIndex = 25;
            this.txtDescription.TextChanged += new System.EventHandler(this.DescriptionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Description";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(197, 298);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 27;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TaskInputEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 333);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.buttEditor);
            this.Controls.Add(this.cmboType);
            this.Controls.Add(this.cmboVariables);
            this.Controls.Add(this.buttAdd);
            this.Controls.Add(this.buttDelete);
            this.Controls.Add(this.cmboInputs);
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttOk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.isLit);
            this.Controls.Add(this.isVar);
            this.Controls.Add(this.chkbRequired);
            this.Controls.Add(this.txtDefault);
            this.Controls.Add(this.lblDefault);
            this.Controls.Add(this.txtJsonPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.txtLit);
            this.Controls.Add(this.lblValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TaskInputEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtLit;
        private System.Windows.Forms.TextBox txtJsonPath;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtDefault;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.CheckBox chkbRequired;
        private System.Windows.Forms.RadioButton isVar;
        private System.Windows.Forms.RadioButton isLit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttOk;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.ComboBox cmboInputs;
        private System.Windows.Forms.Button buttDelete;
        private System.Windows.Forms.Button buttAdd;
        private System.Windows.Forms.ComboBox cmboVariables;
        private System.Windows.Forms.ComboBox cmboType;
        private System.Windows.Forms.Button buttEditor;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}