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
using System.Net;
using System.Windows.Forms;
using FlowMonitor.Models;
using Newtonsoft.Json;
using Remote;

namespace FlowMonitor.ViewModules.Executions
{
    public sealed partial class ExecutionSelectionControl : UserControl
    {
        public delegate void ExecutionSelectedHandler(object sender, ExecutionSelectedEventArgs e);
        public event ExecutionSelectedHandler OnExecutionSelected;
        private List<ExecutionSummary> executions;
        private Font idFont;

        public ExecutionSelectionControl()
        {
            InitializeComponent();
            lstbExecutions.DrawItem += DrawItem;
            lstbExecutions.DrawMode = DrawMode.OwnerDrawFixed;
            lstbExecutions.SelectionMode = SelectionMode.One;

            lstbExecutions.ItemHeight = 50;
            cmboState.SelectedIndex = 0;

            // Preset earliest time to an hour ago
            // TODO: this should be configurable 
            dateStarted.Value = DateTime.UtcNow.AddMonths(-1);
            idFont = new Font(Font.Name, 9, FontStyle.Bold);
        }

        private void DrawItem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var item = (ExecutionSummary)lstbExecutions.Items[e.Index];

            g.FillRectangle(SystemBrushes.Control, e.Bounds);
            var itemRect = e.Bounds;
            itemRect.Inflate(-4, -4);


            if(e.State.HasFlag(DrawItemState.Selected))
                g.FillRectangle(SystemBrushes.ControlDark, itemRect);

            TextRenderer.DrawText(g, item.JobId ?? item.ExecutionId.ToString(), idFont, new Point(itemRect.Left + 5, itemRect.Top + 5), Color.DarkBlue);

            string dateStr;
            if(DateTime.UtcNow - item.Started < new TimeSpan(24, 0, 0))
                dateStr = item.Started.ToLongTimeString();
            else
                dateStr = item.Started.ToLongTimeString() + "    " + item.Started.ToShortDateString();

            TextRenderer.DrawText(g, dateStr, Font, new Point(itemRect.Left + 5, itemRect.Top + 25), Color.Black);

            //e.DrawFocusRectangle();
        }

        private void Search(object sender, EventArgs e)
        {
            string url = "executions?";
            var queries = new List<string>();
            if(!string.IsNullOrWhiteSpace(txtbWorkflow.Text))
                queries.Add("workflow=" + WebUtility.UrlEncode(txtbWorkflow.Text));
            if(!string.IsNullOrWhiteSpace(txtbId.Text))
                queries.Add("jobid=" + WebUtility.UrlEncode(txtbId.Text));
            if(cmboState.Text != "Any")
                queries.Add("state=" + cmboState.Text.ToLower());
            queries.Add("after=" + WebUtility.UrlEncode(dateStarted.Value.ToString("o")));

            url += string.Join("&", queries);

            var json = RestClient.Get(url);
            if(json == null)
                executions = new List<ExecutionSummary>();
            else
                executions = JsonConvert.DeserializeObject<List<ExecutionSummary>>(json);
            executions.Reverse();  // Latest at the top
            lstbExecutions.DataSource = executions;
        }

        private void SelectedExecutionChanged(object sender, EventArgs e)
        { 
            var ea = new ExecutionSelectedEventArgs {ExecutionId = ((ExecutionSummary)lstbExecutions.SelectedItem).ExecutionId };
            OnExecutionSelected?.Invoke(this, ea);
        }
    }

    public class ExecutionSelectedEventArgs
    {
        public Guid ExecutionId;
    }
}
