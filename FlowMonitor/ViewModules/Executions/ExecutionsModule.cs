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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FlowMonitor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remote;

namespace FlowMonitor.ViewModules.Executions
{
    class ExecutionsModule : ViewModule
    {
        private readonly ExecutionSelectionControl selector;
        private readonly RichTextBox viewer;
        private readonly ExecutionDetailControl detail;
        private Execution execution;

        public ExecutionsModule()
        {
            SelectorControl = selector = new ExecutionSelectionControl();
            MainControl = viewer = new RichTextBox();
            DetailControl = detail = new ExecutionDetailControl();

            selector.OnExecutionSelected += ExecutionSelected;

            viewer.Font = new Font("Consolas", 12);
        }

        private void ExecutionSelected(object sender, ExecutionSelectedEventArgs e)
        {
            var json = RestClient.Get($"executions/{e.ExecutionId}");
            if(json == null)
            {
                MessageBox.Show("Execution does not exist", "");
                return;
            }

            execution = JsonConvert.DeserializeObject<Execution>(json);

            RenderExecution();

            json = RestClient.Get($"history?executionid={e.ExecutionId}");

            if(json != null)
                detail.SetHistory(JsonConvert.DeserializeObject<List<History>>(json));

            json = RestClient.Get($"variables?executionid={e.ExecutionId}");

            if(json != null)
                detail.SetVariables((JObject)JsonConvert.DeserializeObject(json));
        }

        private void RenderExecution()
        {
            viewer.Text = "";
            viewer.SelectionTabs = new[] { 300 };
            RenderProperty("JobId");
            RenderProperty("ExecutionId");
            RenderProperty("WorkflowName");
            RenderProperty("WorkflowVersion");
            RenderProperty("State");
            RenderProperty("AwaitingDecision");
            RenderProperty("DeciderToken");
            RenderProperty("DeciderAlarm");
            RenderProperty("DecisionList");
            RenderProperty("HistorySeen");
            RenderProperty("LastSeen");
            RenderProperty("ExecutionStartToCloseTimeout");
            RenderProperty("TaskScheduleToCloseTimeout");
            RenderProperty("TaskScheduleToStartTimeout");
            RenderProperty("TaskStartToCloseTimeout");
        }

        private void RenderProperty(string prop)
        {
            viewer.DeselectAll();
            viewer.SelectionFont = new Font(viewer.SelectionFont, FontStyle.Bold);
            viewer.AppendText(MainForm.SplitCamelCase(prop) + "\t", Color.DarkBlue);
            viewer.SelectionFont = new Font(viewer.SelectionFont, FontStyle.Regular);
            var val = typeof(Execution).GetProperty(prop).GetValue(execution, null);
            viewer.AppendText(val?.ToString() ?? "(null)", Color.Black);
            viewer.AppendText("\n");
        }

        public override void ViewModuleSelected()
        {

        }
    }
}
