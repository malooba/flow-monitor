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
using System.Windows.Forms;
using Diagram.Controls.DiagramControl;
using Diagram.Controls.PropertyEditor;
using Diagram.DiagramModel;
using Remote;
using View = Diagram.DiagramView.View;

namespace FlowMonitor.ViewModules.Workflows
{
    /// <summary>
    /// Display workflows and permit selection for display and editing
    /// </summary>
    class WorkflowsModule : ViewModule
    {
        private readonly WorkflowSelector selector;
        private readonly WorkflowEditor editor;
        private readonly Panel selectorPanel;
        private readonly Panel diagramPanel;
        private readonly PropertyEditor propertyEditor;
        private readonly WorkflowToolBar toolBar;
        private View view;
        private Model model;
        private DiagramControl diagram;
        public bool EditMode { get; private set; }

        public WorkflowsModule()
        {
            SelectorControl = selectorPanel = new Panel();
            selectorPanel.Dock = DockStyle.Fill;
            selector = new WorkflowSelector {Dock = DockStyle.Fill};
            editor = new WorkflowEditor {Dock = DockStyle.Fill};
            SetSelectorPanel(editMode: false);
            MainControl = diagramPanel = new Panel();
            DetailControl = propertyEditor = new PropertyEditor();
            toolBar = new WorkflowToolBar();
            ToolBar = toolBar;

            selector.EditSelectedWorkflow += EditSelectedWorkflow;
            selector.ViewSelectedWorkflow += ViewSelectedWorkflow;
            selector.NewWorkflow += NewWorkflow;
            editor.SaveWorkflow += SaveWorkflow;
            editor.DiscardWorkflow += DiscardWorkflow;
            toolBar.DeleteSelected += DeleteSelected;
            toolBar.Save += SaveWorkflow;
            toolBar.Edit += EditWorkflow;
        }

        // temporary hack
        private void EditWorkflow(object sender, EventArgs e)
        {
            EnableEditing(true);
        }

        public void EnableEditing(bool enable)
        {
            if(diagram != null) diagram.Document.ReadOnly = !enable;
            propertyEditor.ReadOnly = !enable;
            toolBar.EnableEditing = enable;
            EditMode = enable;
        }

        private void SetSelectorPanel(bool editMode)
        {
            selectorPanel.Controls.Clear();
            selectorPanel.Controls.Add(editMode ? editor : (Control)selector);
        }

        private void ViewSelectedWorkflow(object sender, System.EventArgs e)
        {
            OpenWorkflow(selector.SelectedWorkflow);
            EnableEditing(false);
        }

        private void EditSelectedWorkflow(object sender, System.EventArgs e)
        {
            SetSelectorPanel(editMode: true); 
            OpenWorkflow(selector.SelectedWorkflow);
            EnableEditing(true);
        }

        public override void ViewModuleSelected()
        {
            if(!EditMode)
                selector.GetWorkflows();
        }

        private void LoadModel()
        {
            diagramPanel.Controls.Clear();
            propertyEditor.Workflow = Model.Workflow;
            view = new View(model, propertyEditor);
            diagram = new DiagramControl(view)
            {
                Name = "DiagramControl",
                Dock = DockStyle.Fill
            };
            diagramPanel.Controls.Add(diagram);

            model.RouteConnections();
            EnableEditing(enable: true);

            // Text = $"{Model.Workflow.Name} {Model.Workflow.Version}";
        }

        private void SaveWorkflow(object sender, System.EventArgs e)
        {
            if(model.Save())
            {
                SetSelectorPanel(editMode: false);
                EnableEditing(enable: false);
                selector.GetWorkflows();
                EditMode = false;
            }
        }

        private void DiscardWorkflow(object sender, System.EventArgs e)
        {
            SetSelectorPanel(editMode: false);
            EnableEditing(enable: false);
            OpenWorkflow(selector.SelectedWorkflow);
            EditMode = false;
        }

        /*
        private void SaveToFile(object sender, System.EventArgs e)
        {
            saveFileDialog1.FileName = $"{Model.Workflow.Name}_{Model.Workflow.Version}.json";
            if(saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                model.SaveToFile(saveFileDialog1.FileName);
        }

        private void OpenFromFile(object sender, System.EventArgs e)
        {
            if(openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                model = new Model(openFileDialog1.FileName);
                LoadModel();
            }
        }
        */

        private void OpenWorkflow(WorkflowId wf)
        {
            model = new Model(wf.Name, wf.Version);
            LoadModel();
        }

        private void NewWorkflow(object sender, System.EventArgs e)
        {
            model = new Model();
            LoadModel();
            SetSelectorPanel(editMode: true);
            EnableEditing(true);
        }

        private void DeleteSelected(object sender, System.EventArgs e)
        {
            diagram?.Document.DeleteSelected();
        }

        /*
        private void EnableEditItems(object sender, System.EventArgs e)
        {
            deleteSelectedMenuItem.Enabled = (view?.Selected != null);
        }

        private void EnableWorkflowItems(object sender, System.EventArgs e)
        {
            var enable = model != null;
            workflowSaveMenuItem.Enabled = enable;
            workflowSaveFileMenuItem.Enabled = enable;
        }
        */
    }
}
