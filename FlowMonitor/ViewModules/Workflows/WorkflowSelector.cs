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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Remote;

namespace FlowMonitor.ViewModules.Workflows
{
    public partial class WorkflowSelector : UserControl
    {
        public event EventHandler EditSelectedWorkflow;
        public event EventHandler ViewSelectedWorkflow;
        public event EventHandler NewWorkflow;

        public WorkflowSelector()
        {
            InitializeComponent();
            
            lstbWorkflows.SelectedIndexChanged += ViewWorkflow;

            GetWorkflows();
        }

        private void ViewWorkflow(object sender, EventArgs e)
        {
            ViewSelectedWorkflow?.Invoke(this, e);
        }

        public WorkflowId SelectedWorkflow => (WorkflowId)lstbWorkflows.SelectedItem;

        private void EditSelected(object sender, EventArgs e)
        {
            EditSelectedWorkflow?.Invoke(this, e);
        }

        public void GetWorkflows()
        {
            var json = RestClient.Get("workflows");
            if(json == null) return;
            var workflows = JsonConvert.DeserializeObject<List<WorkflowId>>(json);

            if(chkbShowAllVersions.Checked)
                lstbWorkflows.DataSource = workflows;
            else
                lstbWorkflows.DataSource =
                   (from wf in workflows
                    group wf by wf.Name
                    into g
                    let latest = g.OrderBy(x => x.VersionLong).Last()
                    select latest).ToList();
        }

        private void ShowAllVersionsChanged(object sender, EventArgs e)
        {
            GetWorkflows();
        }

        private void DeleteWorkflow(object sender, EventArgs e)
        {
            RestClient.Delete($"/workflows/{SelectedWorkflow.Name}/versions/{SelectedWorkflow.Version}");
            GetWorkflows();
        }

        private void NewWorkflowClick(object sender, EventArgs e)
        {
            NewWorkflow?.Invoke(this, e);
        }
    }
}
