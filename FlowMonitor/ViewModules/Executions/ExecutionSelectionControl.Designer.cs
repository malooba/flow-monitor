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

namespace FlowMonitor.ViewModules.Executions
{
    sealed partial class ExecutionSelectionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtbId = new System.Windows.Forms.TextBox();
            this.dateStarted = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lstbExecutions = new System.Windows.Forms.ListBox();
            this.cmboState = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbWorkflow = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // txtbId
            // 
            this.txtbId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbId.Location = new System.Drawing.Point(70, 41);
            this.txtbId.Name = "txtbId";
            this.txtbId.Size = new System.Drawing.Size(154, 20);
            this.txtbId.TabIndex = 1;
            // 
            // dateStarted
            // 
            this.dateStarted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateStarted.Location = new System.Drawing.Point(70, 67);
            this.dateStarted.Name = "dateStarted";
            this.dateStarted.Size = new System.Drawing.Size(154, 20);
            this.dateStarted.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Started";
            // 
            // lstbExecutions
            // 
            this.lstbExecutions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbExecutions.FormattingEnabled = true;
            this.lstbExecutions.Location = new System.Drawing.Point(15, 126);
            this.lstbExecutions.Name = "lstbExecutions";
            this.lstbExecutions.Size = new System.Drawing.Size(209, 472);
            this.lstbExecutions.TabIndex = 4;
            this.lstbExecutions.SelectedIndexChanged += new System.EventHandler(this.SelectedExecutionChanged);
            // 
            // cmboState
            // 
            this.cmboState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmboState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboState.FormattingEnabled = true;
            this.cmboState.Items.AddRange(new object[] {
            "Any",
            "Running",
            "Paused",
            "Completed",
            "Cancelled",
            "Failed"});
            this.cmboState.Location = new System.Drawing.Point(70, 94);
            this.cmboState.Name = "cmboState";
            this.cmboState.Size = new System.Drawing.Size(154, 21);
            this.cmboState.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "State";
            // 
            // txtbWorkflow
            // 
            this.txtbWorkflow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbWorkflow.Location = new System.Drawing.Point(70, 15);
            this.txtbWorkflow.Name = "txtbWorkflow";
            this.txtbWorkflow.Size = new System.Drawing.Size(154, 20);
            this.txtbWorkflow.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Workflow";
            // 
            // buttSearch
            // 
            this.buttSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttSearch.Location = new System.Drawing.Point(149, 609);
            this.buttSearch.Name = "buttSearch";
            this.buttSearch.Size = new System.Drawing.Size(75, 23);
            this.buttSearch.TabIndex = 9;
            this.buttSearch.Text = "Search";
            this.buttSearch.UseVisualStyleBackColor = true;
            this.buttSearch.Click += new System.EventHandler(this.Search);
            // 
            // ExecutionSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttSearch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbWorkflow);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmboState);
            this.Controls.Add(this.lstbExecutions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateStarted);
            this.Controls.Add(this.txtbId);
            this.Controls.Add(this.label1);
            this.Name = "ExecutionSelectionControl";
            this.Size = new System.Drawing.Size(239, 644);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbId;
        private System.Windows.Forms.DateTimePicker dateStarted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstbExecutions;
        private System.Windows.Forms.ComboBox cmboState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbWorkflow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttSearch;
    }
}
