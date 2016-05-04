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

namespace FlowMonitor.ViewModules.Workflows
{
    partial class WorkflowToolBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkflowToolBar));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbutEdit = new System.Windows.Forms.ToolStripButton();
            this.tbutDeleteSelected = new System.Windows.Forms.ToolStripButton();
            this.tbutSave = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbutEdit,
            this.tbutDeleteSelected,
            this.tbutSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(191, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbutEdit
            // 
            this.tbutEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbutEdit.Image = ((System.Drawing.Image)(resources.GetObject("tbutEdit.Image")));
            this.tbutEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbutEdit.Name = "tbutEdit";
            this.tbutEdit.Size = new System.Drawing.Size(31, 22);
            this.tbutEdit.Text = "Edit";
            this.tbutEdit.Click += new System.EventHandler(this.EditClicked);
            // 
            // tbutDeleteSelected
            // 
            this.tbutDeleteSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbutDeleteSelected.Image = ((System.Drawing.Image)(resources.GetObject("tbutDeleteSelected.Image")));
            this.tbutDeleteSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbutDeleteSelected.Name = "tbutDeleteSelected";
            this.tbutDeleteSelected.Size = new System.Drawing.Size(91, 22);
            this.tbutDeleteSelected.Text = "Delete Selected";
            this.tbutDeleteSelected.Click += new System.EventHandler(this.DeleteSelectedClicked);
            // 
            // tbutSave
            // 
            this.tbutSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbutSave.Image = ((System.Drawing.Image)(resources.GetObject("tbutSave.Image")));
            this.tbutSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbutSave.Name = "tbutSave";
            this.tbutSave.Size = new System.Drawing.Size(35, 22);
            this.tbutSave.Text = "Save";
            this.tbutSave.Click += new System.EventHandler(this.SaveClicked);
            // 
            // WorkflowToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.toolStrip1);
            this.Name = "WorkflowToolBar";
            this.Size = new System.Drawing.Size(191, 25);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbutEdit;
        private System.Windows.Forms.ToolStripButton tbutDeleteSelected;
        private System.Windows.Forms.ToolStripButton tbutSave;
    }
}
