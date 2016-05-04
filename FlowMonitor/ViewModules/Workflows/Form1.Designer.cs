namespace DiagramTest
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.workflowNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workflowOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workflowOpenFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workflowSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workflowSaveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.deleteSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.symbolPalette = new Diagram.Controls.SymbolPalette.SymbolPalette();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(226, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Click += new System.EventHandler(this.DeleteSelected);
            this.splitContainer1.Size = new System.Drawing.Size(1013, 669);
            this.splitContainer1.SplitterDistance = 753;
            this.splitContainer1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1239, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workflowNewMenuItem,
            this.workflowOpenMenuItem,
            this.workflowOpenFileMenuItem,
            this.workflowSaveMenuItem,
            this.workflowSaveFileMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(71, 22);
            this.toolStripDropDownButton1.Text = "Workflow";
            this.toolStripDropDownButton1.DropDownOpening += new System.EventHandler(this.EnableWorkflowItems);
            // 
            // workflowNewMenuItem
            // 
            this.workflowNewMenuItem.Name = "workflowNewMenuItem";
            this.workflowNewMenuItem.Size = new System.Drawing.Size(164, 22);
            this.workflowNewMenuItem.Text = "New";
            this.workflowNewMenuItem.Click += new System.EventHandler(this.NewWorkflow);
            // 
            // workflowOpenMenuItem
            // 
            this.workflowOpenMenuItem.Name = "workflowOpenMenuItem";
            this.workflowOpenMenuItem.Size = new System.Drawing.Size(164, 22);
            this.workflowOpenMenuItem.Text = "Open...";
            this.workflowOpenMenuItem.Click += new System.EventHandler(this.OpenWorkflow);
            // 
            // workflowOpenFileMenuItem
            // 
            this.workflowOpenFileMenuItem.Name = "workflowOpenFileMenuItem";
            this.workflowOpenFileMenuItem.Size = new System.Drawing.Size(164, 22);
            this.workflowOpenFileMenuItem.Text = "Open From File...";
            this.workflowOpenFileMenuItem.Click += new System.EventHandler(this.OpenFromFile);
            // 
            // workflowSaveMenuItem
            // 
            this.workflowSaveMenuItem.Name = "workflowSaveMenuItem";
            this.workflowSaveMenuItem.Size = new System.Drawing.Size(164, 22);
            this.workflowSaveMenuItem.Text = "Save...";
            this.workflowSaveMenuItem.Click += new System.EventHandler(this.SaveWorkflow);
            // 
            // workflowSaveFileMenuItem
            // 
            this.workflowSaveFileMenuItem.Name = "workflowSaveFileMenuItem";
            this.workflowSaveFileMenuItem.Size = new System.Drawing.Size(164, 22);
            this.workflowSaveFileMenuItem.Text = "Save To File...";
            this.workflowSaveFileMenuItem.Click += new System.EventHandler(this.SaveToFile);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(40, 22);
            this.toolStripDropDownButton2.Text = "Edit";
            this.toolStripDropDownButton2.DropDownOpening += new System.EventHandler(this.EnableEditItems);
            // 
            // deleteSelectedMenuItem
            // 
            this.deleteSelectedMenuItem.Name = "deleteSelectedMenuItem";
            this.deleteSelectedMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteSelectedMenuItem.Text = "Delete Selected";
            this.deleteSelectedMenuItem.Click += new System.EventHandler(this.DeleteSelected);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Workflow File|*.json|All Files|*.*";
            this.saveFileDialog1.Title = "Save Workflow to File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Workflow File|*.json|All Files|*.*";
            this.openFileDialog1.Title = "Open Workflow from File";
            // 
            // symbolPalette
            // 
            this.symbolPalette.Dock = System.Windows.Forms.DockStyle.Left;
            this.symbolPalette.ImageSize = new System.Drawing.Size(32, 32);
            this.symbolPalette.Location = new System.Drawing.Point(0, 25);
            this.symbolPalette.Name = "symbolPalette";
            this.symbolPalette.Size = new System.Drawing.Size(226, 669);
            this.symbolPalette.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 694);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.symbolPalette);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Diagram.Controls.SymbolPalette.SymbolPalette symbolPalette;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem workflowNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workflowOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workflowOpenFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workflowSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workflowSaveFileMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedMenuItem;

    }
}

