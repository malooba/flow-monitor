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

namespace Diagram.Controls.DiagramControl
{
    sealed partial class DiagramControl
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
            this.document = new Diagram.Controls.DiagramControl.Document();
            this.SuspendLayout();
            // 
            // document
            // 
            this.document.AllowDrop = true;
            this.document.BackColor = System.Drawing.SystemColors.Window;
            this.document.Cursor = System.Windows.Forms.Cursors.Default;
            this.document.Location = new System.Drawing.Point(0, 0);
            this.document.Name = "document";
            this.document.ReadOnly = false;
            this.document.Size = new System.Drawing.Size(121, 110);
            this.document.TabIndex = 0;
            this.document.View = null;
            this.document.Zoom = 1F;
            // 
            // DiagramControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.document);
            this.Name = "DiagramControl";
            this.ResumeLayout(false);

        }

        #endregion

        private Document document;
    }
}
