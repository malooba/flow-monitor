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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FlowMonitor.Properties;
using FlowMonitor.ViewModules;
using FlowMonitor.ViewModules.Executions;
using FlowMonitor.ViewModules.TaskLists;
using FlowMonitor.ViewModules.Workflows;

namespace FlowMonitor
{
    public partial class MainForm : Form
    {
        private readonly WorkflowsModule workflowsModule;
        private readonly ExecutionsModule executionsModule;
        private readonly TaskListsModule taskListsModule;
        
        private ViewModule module;
        private Control ToolBar;

        private static readonly Regex splitter = new Regex("(?<!^)([A-Z])");

        public MainForm()
        {
            InitializeComponent();
            toolStrip1.Renderer = new ButtonStateRenderer();

            workflowsModule = new WorkflowsModule();
            executionsModule = new ExecutionsModule();
            taskListsModule = new TaskListsModule();
            SelectView(buttWorkflows, EventArgs.Empty);
        }

        private void ToggleLeftPanel(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
            buttLeftPanel.Image = splitContainer1.Panel1Collapsed ? Resources.Expand : Resources.Collapse;
        }

        private void ToggleRightPanel(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
            buttRightPanel.Image = splitContainer2.Panel2Collapsed ? Resources.Expand : Resources.Collapse;
        }

        private void SelectView(object sender, EventArgs e)
        {
            if(sender != buttWorkflows) buttWorkflows.Checked = false;
            if(sender != buttExecutions) buttExecutions.Checked = false;
            if(sender != buttTaskLists) buttTaskLists.Checked = false;
            if(sender != buttLogs) buttLogs.Checked = false;
            ((ToolStripButton)sender).Checked = true;

            // TODO: Select the appropriate view module
            if(sender == buttWorkflows)
                module = workflowsModule;
            else if(sender == buttExecutions)
                module = executionsModule;
            else if(sender == buttTaskLists)
                module = taskListsModule;
            else
                module = null;

            SelectModule();
        }

        private void SelectModule()
        {
            panel1.Controls.Clear();
            panel2.Controls.Clear();
            panel3.Controls.Clear();
            if(module == null)
                return;
            panel1.Controls.Add(module.SelectorControl);
            module.SelectorControl.Dock = DockStyle.Fill;
            panel1.Invalidate();
            panel2.Controls.Add(module.MainControl);
            module.MainControl.Dock = DockStyle.Fill;
            panel2.Invalidate();
            panel3.Controls.Add(module.DetailControl);
            module.DetailControl.Dock = DockStyle.Fill;
            panel3.Invalidate();
            module.ViewModuleSelected();

            if (ToolBar != null)
            {
                panlMenu.Controls.Remove(ToolBar);
            }
            ToolBar = module.ToolBar;
            if (ToolBar != null)
            {
                panlMenu.Controls.Add(ToolBar);
                ToolBar.BringToFront();
                ToolBar.Dock = DockStyle.Left;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(workflowsModule.EditMode)
            {
                module = workflowsModule;
                SelectModule();
                MessageBox.Show("Unsaved workflow edit\r\nPlease save or discard", "Warning", MessageBoxButtons.OK);
                e.Cancel = true;
            }
        }

        public static string SplitCamelCase(string cc)
        {
            return splitter.Replace(cc, " $1");
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
