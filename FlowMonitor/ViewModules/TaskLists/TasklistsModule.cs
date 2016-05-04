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
using FlowMonitor.ViewModules.Executions;
using Newtonsoft.Json;
using Remote;

namespace FlowMonitor.ViewModules.TaskLists
{
    class TaskListsModule : ViewModule
    {
        private readonly ListBox selector;
        private readonly ListBox viewer;
        private readonly RichTextBox detail;
        private readonly Font idFont;

        public TaskListsModule()
        {
            SelectorControl = selector = new ListBox();
            MainControl = viewer = new ListBox();
            DetailControl = detail = new RichTextBox(); 

            viewer.DrawItem += DrawTaskListItem;
            viewer.DrawMode = DrawMode.OwnerDrawFixed;
            viewer.SelectionMode = SelectionMode.One;
            viewer.ItemHeight = 50;
            viewer.Font = new Font("Consolas", 9);
            detail.Font = viewer.Font;
            idFont = new Font(viewer.Font.Name, 9, FontStyle.Bold);

            selector.SelectedIndexChanged += TaskListChanged;
            viewer.SelectedIndexChanged += TaskChanged;
        }

        private void DrawTaskListItem(object sender, DrawItemEventArgs e)
        {
            if(e.Index < 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var item = (TaskListItem)viewer.Items[e.Index];

            g.FillRectangle(SystemBrushes.Control, e.Bounds);
            var itemRect = e.Bounds;
            itemRect.Inflate(-4, -4);


            if(e.State.HasFlag(DrawItemState.Selected))
                g.FillRectangle(SystemBrushes.ControlDark, itemRect);

            string header;
            if(item.SchedulingEvent != null)
                header = $"Workflow: {item.WorkflowName} v{item.WorkflowVersion} - Task: {item.SchedulingEvent.TaskId} - Activity: {item.SchedulingEvent.ActivityName} v{item.SchedulingEvent?.ActivityVersion} - Job ID: {item.JobId ?? item.ExecutionId.ToString()}";
            else
                header = $"Workflow: {item.WorkflowName} v{item.WorkflowVersion} - Notification - Job ID: {item.JobId ?? item.ExecutionId.ToString()}";
            TextRenderer.DrawText(g, header, idFont, new Point(itemRect.Left + 5, itemRect.Top + 5), Color.DarkBlue);

            var dateStr = FormatDateTime(item.ScheduledAt);

            TextRenderer.DrawText(g, "Scheduled At: " + dateStr, viewer.Font, new Point(itemRect.Left + 5, itemRect.Top + 25), Color.Black);

            dateStr = item.StartedAt.HasValue ? FormatDateTime(item.StartedAt.Value) : "";

            TextRenderer.DrawText(g, "Started At: " + dateStr, viewer.Font, new Point(itemRect.Left + 220, itemRect.Top + 25), Color.Black);
        }

        private string FormatDateTime(DateTime dt)
        {
            if(DateTime.UtcNow - dt < new TimeSpan(24, 0, 0))
                return dt.ToLongTimeString();
            return dt.ToLongTimeString() + "    " + dt.ToShortDateString();
        }

        private void TaskListChanged(object sender, EventArgs e)
        {
            var list = (string)selector.SelectedItem; 
            var json = RestClient.Get($"/tasks?list={list}");
            if(json != null)
            {
                var tasks = JsonConvert.DeserializeObject<List<TaskListItem>>(json);
                viewer.DataSource = tasks;
            }
        }

        public override void ViewModuleSelected()
        {
            var json = RestClient.Get("/tasklists");
            if(json != null)
            {
                var tasklists = JsonConvert.DeserializeObject<List<string>>(json);
                selector.DataSource = tasklists; 
            }
        }

        private void TaskChanged(object sender, EventArgs e)
        {
            var t = (TaskListItem)viewer.SelectedItem;

            detail.Text = "";
            detail.SelectAll();
            detail.SelectionTabs = new[] { 200 };

            
            foreach(var prop in t.GetType().GetProperties())
            {
                if(prop.Name == "SchedulingEvent") continue;
                var val = prop.GetValue(t);
                var json = JsonConvert.SerializeObject(val, Formatting.Indented);
                var label = MainForm.SplitCamelCase(prop.Name);
                if(json.StartsWith("{") || json.StartsWith("["))
                    ShowObjectProperty(label, json);
                else
                    ShowProperty(label, json);
            }

            var se = t.SchedulingEvent;
            if(se != null)
            {
                detail.SelectionFont = new Font(detail.SelectionFont, FontStyle.Bold);
                detail.AppendText("\nScheduling Event\n", Color.DarkBlue);
                foreach(var prop in se.GetType().GetProperties())
                {
                    var val = prop.GetValue(se);
                    var json = JsonConvert.SerializeObject(val, Formatting.Indented);
                    var label = MainForm.SplitCamelCase(prop.Name);
                    if(json.StartsWith("{") || json.StartsWith("["))
                        ShowObjectProperty(label, json);
                    else
                        ShowProperty(label, json);
                }
            }
        }

        private void ShowProperty(string label, string content)
        {
            detail.SelectionFont = new Font(detail.SelectionFont, FontStyle.Bold);
            detail.AppendText(label + "\t", Color.DarkBlue);
            detail.SelectionFont = new Font(detail.SelectionFont, FontStyle.Regular);
            detail.AppendText(content + "\n", Color.Black);
        }

        private void ShowObjectProperty(string label, string content)
        {
            detail.SelectionFont = new Font(detail.SelectionFont, FontStyle.Bold);
            detail.AppendText(label + ":\n", Color.DarkBlue);
            detail.SelectionFont = new Font(detail.SelectionFont, FontStyle.Regular);
            detail.AppendText(content + "\n", Color.Black);
        }
    }
}
