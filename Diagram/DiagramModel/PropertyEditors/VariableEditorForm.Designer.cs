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
    partial class VariableEditorForm
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
            this.txtJsonPath = new Diagram.DiagramModel.PropertyEditors.JsonPathInput();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtDefault = new System.Windows.Forms.TextBox();
            this.lblDefault = new System.Windows.Forms.Label();
            this.chkbRequired = new System.Windows.Forms.CheckBox();
            this.isInput = new System.Windows.Forms.RadioButton();
            this.isLit = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.buttOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.cmboVariables = new System.Windows.Forms.ComboBox();
            this.buttDelete = new System.Windows.Forms.Button();
            this.buttAdd = new System.Windows.Forms.Button();
            this.cmboType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLit = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtJsonPath
            // 
            this.txtJsonPath.Location = new System.Drawing.Point(79, 45);
            this.txtJsonPath.Name = "txtJsonPath";
            this.txtJsonPath.Size = new System.Drawing.Size(174, 20);
            this.txtJsonPath.TabIndex = 3;
            this.txtJsonPath.TextChanged += new System.EventHandler(this.PathChanged);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(6, 48);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(60, 13);
            this.lblPath.TabIndex = 2;
            this.lblPath.Text = "JSON Path";
            // 
            // txtDefault
            // 
            this.txtDefault.Location = new System.Drawing.Point(79, 91);
            this.txtDefault.Name = "txtDefault";
            this.txtDefault.Size = new System.Drawing.Size(174, 20);
            this.txtDefault.TabIndex = 5;
            this.txtDefault.TextChanged += new System.EventHandler(this.DefaultChanged);
            // 
            // lblDefault
            // 
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new System.Drawing.Point(6, 94);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(41, 13);
            this.lblDefault.TabIndex = 4;
            this.lblDefault.Text = "Default";
            // 
            // chkbRequired
            // 
            this.chkbRequired.AutoSize = true;
            this.chkbRequired.Location = new System.Drawing.Point(79, 71);
            this.chkbRequired.Name = "chkbRequired";
            this.chkbRequired.Size = new System.Drawing.Size(15, 14);
            this.chkbRequired.TabIndex = 7;
            this.chkbRequired.UseVisualStyleBackColor = true;
            this.chkbRequired.CheckedChanged += new System.EventHandler(this.RequiredChanged);
            // 
            // isInput
            // 
            this.isInput.AutoSize = true;
            this.isInput.Checked = true;
            this.isInput.Location = new System.Drawing.Point(9, 19);
            this.isInput.Name = "isInput";
            this.isInput.Size = new System.Drawing.Size(75, 17);
            this.isInput.TabIndex = 8;
            this.isInput.TabStop = true;
            this.isInput.Text = "From Input";
            this.isInput.UseVisualStyleBackColor = true;
            this.isInput.CheckedChanged += new System.EventHandler(this.InputSourceChanged);
            // 
            // isLit
            // 
            this.isLit.AutoSize = true;
            this.isLit.Location = new System.Drawing.Point(104, 19);
            this.isLit.Name = "isLit";
            this.isLit.Size = new System.Drawing.Size(53, 17);
            this.isLit.TabIndex = 9;
            this.isLit.Text = "Literal";
            this.isLit.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Variable:";
            // 
            // buttOK
            // 
            this.buttOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttOK.Location = new System.Drawing.Point(197, 283);
            this.buttOK.Name = "buttOK";
            this.buttOK.Size = new System.Drawing.Size(75, 23);
            this.buttOK.TabIndex = 11;
            this.buttOK.Text = "OK";
            this.buttOK.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Type: ";
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(6, 71);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 16;
            this.lblRequired.Text = "Required";
            // 
            // cmboVariables
            // 
            this.cmboVariables.DisplayMember = "Key";
            this.cmboVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboVariables.FormattingEnabled = true;
            this.cmboVariables.Location = new System.Drawing.Point(69, 11);
            this.cmboVariables.Name = "cmboVariables";
            this.cmboVariables.Size = new System.Drawing.Size(148, 21);
            this.cmboVariables.TabIndex = 17;
            this.cmboVariables.SelectionChangeCommitted += new System.EventHandler(this.VariableChanged);
            // 
            // buttDelete
            // 
            this.buttDelete.Location = new System.Drawing.Point(223, 38);
            this.buttDelete.Name = "buttDelete";
            this.buttDelete.Size = new System.Drawing.Size(49, 23);
            this.buttDelete.TabIndex = 19;
            this.buttDelete.Text = "Delete";
            this.buttDelete.UseVisualStyleBackColor = true;
            this.buttDelete.Click += new System.EventHandler(this.DeleteVariable);
            // 
            // buttAdd
            // 
            this.buttAdd.Location = new System.Drawing.Point(223, 9);
            this.buttAdd.Name = "buttAdd";
            this.buttAdd.Size = new System.Drawing.Size(48, 23);
            this.buttAdd.TabIndex = 20;
            this.buttAdd.Text = "Add";
            this.buttAdd.UseVisualStyleBackColor = true;
            this.buttAdd.Click += new System.EventHandler(this.AddVariable);
            // 
            // cmboType
            // 
            this.cmboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboType.FormattingEnabled = true;
            this.cmboType.Location = new System.Drawing.Point(69, 38);
            this.cmboType.Name = "cmboType";
            this.cmboType.Size = new System.Drawing.Size(148, 21);
            this.cmboType.TabIndex = 21;
            this.cmboType.SelectedIndexChanged += new System.EventHandler(this.TypeChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLit);
            this.groupBox1.Controls.Add(this.txtDefault);
            this.groupBox1.Controls.Add(this.lblPath);
            this.groupBox1.Controls.Add(this.txtJsonPath);
            this.groupBox1.Controls.Add(this.lblRequired);
            this.groupBox1.Controls.Add(this.lblDefault);
            this.groupBox1.Controls.Add(this.chkbRequired);
            this.groupBox1.Controls.Add(this.isInput);
            this.groupBox1.Controls.Add(this.isLit);
            this.groupBox1.Location = new System.Drawing.Point(13, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 128);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initial Value";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(13, 92);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(258, 51);
            this.txtDescription.TabIndex = 23;
            this.txtDescription.TextChanged += new System.EventHandler(this.DescriptionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Description";
            // 
            // txtLit
            // 
            this.txtLit.Location = new System.Drawing.Point(79, 45);
            this.txtLit.Name = "txtLit";
            this.txtLit.Size = new System.Drawing.Size(174, 20);
            this.txtLit.TabIndex = 17;
            this.txtLit.TextChanged += new System.EventHandler(this.LitChanged);
            this.txtLit.Validating += new System.ComponentModel.CancelEventHandler(this.LitValidating);
            // 
            // VariableEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 318);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmboType);
            this.Controls.Add(this.buttAdd);
            this.Controls.Add(this.buttDelete);
            this.Controls.Add(this.cmboVariables);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttOK);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "VariableEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtDefault;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.CheckBox chkbRequired;
        private System.Windows.Forms.RadioButton isInput;
        private System.Windows.Forms.RadioButton isLit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.ComboBox cmboVariables;
        private System.Windows.Forms.Button buttDelete;
        private System.Windows.Forms.Button buttAdd;
        private System.Windows.Forms.ComboBox cmboType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLit;
        private JsonPathInput txtJsonPath;
    }
}