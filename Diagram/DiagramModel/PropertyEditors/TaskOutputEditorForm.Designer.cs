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
    partial class TaskOutputEditorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmboOutputs = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmboVariables = new System.Windows.Forms.ComboBox();
            this.buttOk = new System.Windows.Forms.Button();
            this.buttAdd = new System.Windows.Forms.Button();
            this.buttDelete = new System.Windows.Forms.Button();
            this.cmboType = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type";
            // 
            // cmboOutputs
            // 
            this.cmboOutputs.DisplayMember = "Key";
            this.cmboOutputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboOutputs.FormattingEnabled = true;
            this.cmboOutputs.Location = new System.Drawing.Point(97, 11);
            this.cmboOutputs.Name = "cmboOutputs";
            this.cmboOutputs.Size = new System.Drawing.Size(158, 21);
            this.cmboOutputs.TabIndex = 2;
            this.cmboOutputs.SelectedIndexChanged += new System.EventHandler(this.OutputChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Variable Name";
            // 
            // cmboVariables
            // 
            this.cmboVariables.DisplayMember = "Key";
            this.cmboVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboVariables.FormattingEnabled = true;
            this.cmboVariables.Location = new System.Drawing.Point(97, 159);
            this.cmboVariables.Name = "cmboVariables";
            this.cmboVariables.Size = new System.Drawing.Size(158, 21);
            this.cmboVariables.TabIndex = 5;
            this.cmboVariables.SelectedIndexChanged += new System.EventHandler(this.UpdateOutput);
            // 
            // buttOk
            // 
            this.buttOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttOk.Location = new System.Drawing.Point(160, 196);
            this.buttOk.Name = "buttOk";
            this.buttOk.Size = new System.Drawing.Size(75, 23);
            this.buttOk.TabIndex = 6;
            this.buttOk.Text = "OK";
            this.buttOk.UseVisualStyleBackColor = true;
            // 
            // buttAdd
            // 
            this.buttAdd.Location = new System.Drawing.Point(267, 10);
            this.buttAdd.Name = "buttAdd";
            this.buttAdd.Size = new System.Drawing.Size(48, 23);
            this.buttAdd.TabIndex = 7;
            this.buttAdd.Text = "Add";
            this.buttAdd.UseVisualStyleBackColor = true;
            this.buttAdd.Click += new System.EventHandler(this.AddOutput);
            // 
            // buttDelete
            // 
            this.buttDelete.Location = new System.Drawing.Point(267, 39);
            this.buttDelete.Name = "buttDelete";
            this.buttDelete.Size = new System.Drawing.Size(48, 23);
            this.buttDelete.TabIndex = 8;
            this.buttDelete.Text = "Delete";
            this.buttDelete.UseVisualStyleBackColor = true;
            this.buttDelete.Click += new System.EventHandler(this.DeleteOutput);
            // 
            // cmboType
            // 
            this.cmboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboType.FormattingEnabled = true;
            this.cmboType.Location = new System.Drawing.Point(97, 41);
            this.cmboType.Name = "cmboType";
            this.cmboType.Size = new System.Drawing.Size(158, 21);
            this.cmboType.TabIndex = 9;
            this.cmboType.SelectedIndexChanged += new System.EventHandler(this.UpdateOutput);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(15, 90);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(301, 57);
            this.txtDescription.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Description";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(241, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TaskOutputEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 231);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.buttOk);
            this.Controls.Add(this.cmboVariables);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmboType);
            this.Controls.Add(this.buttDelete);
            this.Controls.Add(this.buttAdd);
            this.Controls.Add(this.cmboOutputs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TaskOutputEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmboOutputs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmboVariables;
        private System.Windows.Forms.Button buttOk;
        private System.Windows.Forms.Button buttAdd;
        private System.Windows.Forms.Button buttDelete;
        private System.Windows.Forms.ComboBox cmboType;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}