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

namespace FlowMonitor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttWorkflows = new System.Windows.Forms.ToolStripButton();
            this.buttExecutions = new System.Windows.Forms.ToolStripButton();
            this.buttTaskLists = new System.Windows.Forms.ToolStripButton();
            this.buttLogs = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.buttRightPanel = new System.Windows.Forms.ToolStripButton();
            this.panlMenu = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.buttLeftPanel = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.panlMenu.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttWorkflows,
            this.buttExecutions,
            this.buttTaskLists,
            this.buttLogs});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(24, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // buttWorkflows
            // 
            this.buttWorkflows.AutoSize = false;
            this.buttWorkflows.Checked = true;
            this.buttWorkflows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buttWorkflows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttWorkflows.Image = ((System.Drawing.Image)(resources.GetObject("buttWorkflows.Image")));
            this.buttWorkflows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttWorkflows.Name = "buttWorkflows";
            this.buttWorkflows.Size = new System.Drawing.Size(70, 22);
            this.buttWorkflows.Text = "Workflows";
            this.buttWorkflows.Click += new System.EventHandler(this.SelectView);
            // 
            // buttExecutions
            // 
            this.buttExecutions.AutoSize = false;
            this.buttExecutions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttExecutions.Image = ((System.Drawing.Image)(resources.GetObject("buttExecutions.Image")));
            this.buttExecutions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttExecutions.Name = "buttExecutions";
            this.buttExecutions.Size = new System.Drawing.Size(70, 22);
            this.buttExecutions.Text = "Executions";
            this.buttExecutions.Click += new System.EventHandler(this.SelectView);
            // 
            // buttTaskLists
            // 
            this.buttTaskLists.AutoSize = false;
            this.buttTaskLists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttTaskLists.Image = ((System.Drawing.Image)(resources.GetObject("buttTaskLists.Image")));
            this.buttTaskLists.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttTaskLists.Name = "buttTaskLists";
            this.buttTaskLists.Size = new System.Drawing.Size(70, 22);
            this.buttTaskLists.Text = "TaskLists";
            this.buttTaskLists.Click += new System.EventHandler(this.SelectView);
            // 
            // buttLogs
            // 
            this.buttLogs.AutoSize = false;
            this.buttLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttLogs.Image = ((System.Drawing.Image)(resources.GetObject("buttLogs.Image")));
            this.buttLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttLogs.Name = "buttLogs";
            this.buttLogs.Size = new System.Drawing.Size(70, 22);
            this.buttLogs.Text = "Logs";
            this.buttLogs.Click += new System.EventHandler(this.SelectView);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 629);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1452, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttRightPanel});
            this.toolStrip3.Location = new System.Drawing.Point(1428, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(24, 28);
            this.toolStrip3.TabIndex = 4;
            // 
            // buttRightPanel
            // 
            this.buttRightPanel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.buttRightPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttRightPanel.Image = Properties.Resources.Collapse;
            this.buttRightPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttRightPanel.Name = "buttRightPanel";
            this.buttRightPanel.Size = new System.Drawing.Size(21, 20);
            this.buttRightPanel.Text = "toolStripButton2";
            this.buttRightPanel.Click += new System.EventHandler(this.ToggleRightPanel);
            // 
            // panlMenu
            // 
            this.panlMenu.Controls.Add(this.toolStrip1);
            this.panlMenu.Controls.Add(this.toolStrip3);
            this.panlMenu.Controls.Add(this.toolStrip2);
            this.panlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panlMenu.Location = new System.Drawing.Point(0, 0);
            this.panlMenu.Name = "panlMenu";
            this.panlMenu.Size = new System.Drawing.Size(1452, 28);
            this.panlMenu.TabIndex = 5;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttLeftPanel});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(24, 28);
            this.toolStrip2.TabIndex = 2;
            // 
            // buttLeftPanel
            // 
            this.buttLeftPanel.CheckOnClick = true;
            this.buttLeftPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttLeftPanel.Image = Properties.Resources.Collapse;
            this.buttLeftPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttLeftPanel.Name = "buttLeftPanel";
            this.buttLeftPanel.Size = new System.Drawing.Size(21, 20);
            this.buttLeftPanel.Text = "toolStripButton1";
            this.buttLeftPanel.Click += new System.EventHandler(this.ToggleLeftPanel);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1MinSize = 180;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1452, 601);
            this.splitContainer1.SplitterDistance = 361;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 6;
            this.splitContainer1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(361, 601);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel3);
            this.splitContainer2.Panel2MinSize = 180;
            this.splitContainer2.Size = new System.Drawing.Size(1085, 601);
            this.splitContainer2.SplitterDistance = 730;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(730, 601);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(349, 601);
            this.panel3.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1452, 651);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panlMenu);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Flow Monitor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.panlMenu.ResumeLayout(false);
            this.panlMenu.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton buttWorkflows;
        private System.Windows.Forms.ToolStripButton buttExecutions;
        private System.Windows.Forms.ToolStripButton buttTaskLists;
        private System.Windows.Forms.ToolStripButton buttLogs;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton buttRightPanel;
        private System.Windows.Forms.Panel panlMenu;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton buttLeftPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}

