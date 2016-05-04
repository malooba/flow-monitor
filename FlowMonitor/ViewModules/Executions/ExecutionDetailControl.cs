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
using System.Drawing;
using System.Windows.Forms;
using FlowMonitor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlowMonitor.ViewModules.Executions
{
    public partial class ExecutionDetailControl : UserControl
    {
        public ExecutionDetailControl()
        {
            InitializeComponent();
            rtxtHistoryDetail.Font = new Font("Consolas", 9);
        }

        public void SetHistory(List<History> history)
        {
            lstbHistory.DataSource = history;
        }

        public void SetVariables(JObject vars)
        {
            rtxtVariableDetail.Text = "";
            rtxtVariableDetail.SelectAll();
            rtxtVariableDetail.SelectionTabs = new[] {200};
            foreach(var prop in vars.Properties())
            {
                var json = JsonConvert.SerializeObject(prop.Value, Formatting.Indented);
                if(prop.Value.Type == JTokenType.Array || prop.Value.Type == JTokenType.Object)
                    ShowObjectProperty(prop.Name, json, rtxtVariableDetail);
                else
                    ShowProperty(prop.Name, json, rtxtVariableDetail);
            }
        }

        private void ShowEvent(object sender, EventArgs e)
        {
            History h = (History)lstbHistory.SelectedItem;
           
            rtxtHistoryDetail.Text = "";
            rtxtHistoryDetail.SelectAll();
            rtxtHistoryDetail.SelectionTabs = new[] { 200 };
            rtxtHistoryDetail.SelectionFont = new Font(rtxtHistoryDetail.SelectionFont.FontFamily, 12, FontStyle.Bold);
            rtxtHistoryDetail.AppendText(MainForm.SplitCamelCase(h.EventType) + "\n", Color.DarkBlue);
            ShowProperty("ID", h.Id.ToString(), rtxtHistoryDetail);
            ShowProperty("Timestamp", h.Timestamp.ToString("f"), rtxtHistoryDetail);
            var attributes = (JObject)h.Attributes;
            foreach(var prop in attributes.Properties())
            {
                var json = JsonConvert.SerializeObject(prop.Value, Formatting.Indented);
                if(prop.Value.Type == JTokenType.Array || prop.Value.Type == JTokenType.Object)
                    ShowObjectProperty(prop.Name, json, rtxtHistoryDetail);
                else
                    ShowProperty(prop.Name, json, rtxtHistoryDetail);

            }

        }

        private void ShowProperty(string label, string content, RichTextBox richTextBox)
        {
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, FontStyle.Bold);
            richTextBox.AppendText(label + "\t", Color.DarkBlue);
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, FontStyle.Regular);
            richTextBox.AppendText(content + "\n", Color.Black);
        }

        private void ShowObjectProperty(string label, string content, RichTextBox richTextBox)
        {
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, FontStyle.Bold);
            richTextBox.AppendText(label + ":\n", Color.DarkBlue);
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, FontStyle.Regular);
            richTextBox.AppendText(content + "\n", Color.Black);
        }
    }
}
