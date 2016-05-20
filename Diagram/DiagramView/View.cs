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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Diagram.Controls.PropertyEditor;
using Diagram.DiagramModel;
using Newtonsoft.Json;
using Remote;

namespace Diagram.DiagramView
{
    public class View
    {
        internal readonly Model Model; 
        private readonly PropertyEditor propertyEditor;
        public const int GRID_SIZE = 10;

        public delegate void GraphicsChangedEventHandler(object sender, UpdateGraphicsEventArgs e);

        public event GraphicsChangedEventHandler GraphicsChanged;

        public static Selection Selection;

        public View(Model model, PropertyEditor propertyEditor)
        {
            Model = model;
            Selection = new Selection(this);
            this.propertyEditor = propertyEditor;
            // Make a shape for each task
            foreach(var task in Model.Workflow.Tasks)
            {
                CreateShape(task);
                task.TaskChanged += UpdateGraphics;
            }
	        Connector.ClearConnectors();
            model.CreateConnections();
        }

        public void ShowProperties(object obj, bool showWorkflow = false)
        {
            if(obj == null)
                propertyEditor.SetSelection(showWorkflow ? Model.Workflow : null);
            else
                propertyEditor.SetSelection(obj);
        }

        private void UpdateGraphics(object sender, EventArgs e)
        {
            Repaint(((TaskObj)sender).Symbol.Shape.Bounds);
        }

        private void Repaint(Rectangle rect)
        {
            GraphicsChanged?.Invoke(this, new UpdateGraphicsEventArgs { DamageRect = rect });
        }

        /// <summary>
        ///	Render the view 
        /// </summary>
        /// <param name="g">Graphics context</param>

        public void Render(Graphics g)
        {
            foreach(var connector in Connector.Connectors.Where(c => c.Routed && !c.Selected))
                connector.Render(g);

            // Always render the selected connector(s) last
            foreach(var connector in Selection.Items.OfType<Connector>())
                connector.Render(g);

            foreach(var task in Model.Workflow.Tasks)
                task.Symbol.Shape.Render(g);

            // DEBUG
            // Model.Router?.RenderMap(g);
        }

        /// <summary>
        /// Create a task, its symbol data and shape when a Palette symbol is dropped on the diagram
        /// </summary>
        /// <param name="symbolType"></param>
        /// <param name="datum"></param>
        public TaskObj CreateTask(PaletteObj symbolType, Point datum)
        {
            var symbolTask = symbolType.Task;
            if(symbolTask.ActivityName == "cleanup" && Model.Workflow.Tasks.Any(t => t.ActivityName == "cleanup"))
            {
                MessageBox.Show("No more than one Cleanup Start symbol permitted");
                return null;
            }
	        ActivityObj activity = null;
	        if (symbolTask.HasActivity())
	        {
                var json = RestClient.Get($"/activities/{symbolTask.ActivityName}/versions/{symbolTask.ActivityVersion}");
                activity = JsonConvert.DeserializeObject<ActivityObj>(json);

                if(activity == null)
		        {
			        MessageBox.Show($"Missing activity - {symbolTask.ActivityName} {symbolTask.ActivityVersion}", "Database error");
			        return null;
		        }
	        }

	        var task = new TaskObj
            {
                TaskId = Guid.NewGuid().ToString().Substring(0, 8),
                ActivityName = symbolTask.ActivityName,
                ActivityVersion = symbolTask.ActivityVersion,
                Outflows = (from f in symbolTask.Outflows
                    select new FlowObj {Name = f.Name}).ToArray(),
                Symbol = CloneSymbol(symbolTask.Symbol),
                FailOutflow = new FlowObj { Name = "Err" },
                HiddenProperties = symbolTask.HiddenProperties
            };

			if(activity != null)
			{
				task.Inputs = new InputCollection(activity.Inputs);
                if(task.Outflows.Length != 0)
				    task.Outputs = new OutputCollection(activity.Outputs);
			}

            // Override or add inputs with data from palette symbol
            if(symbolTask.Inputs != null)
                foreach(var input in symbolTask.Inputs)
                    task.Inputs[input.Key] = input.Value;

            // Override or add outputs with data from palette symbol
            if(symbolTask.Outputs != null)
                foreach(var output in symbolTask.Outputs)
                    task.Outputs[output.Key] = output.Value;

            task.Symbol.Shape = Shape.Create(task.Symbol.Style, task, datum);

            if(symbolTask.HasFailOutflow())
            {
                task.FailOutflow = new FlowObj {Name = "Err"};
            }

            Model.Workflow.Tasks.Add(task);
            task.TaskChanged += UpdateGraphics;

            return task;
        }

        /// <summary>
        /// Create a Shape for an existing task
        /// </summary>
        /// <param name="task"></param>
        private static void CreateShape(TaskObj task)
        {
            if(task.Symbol.Shape == null)
            {
                var datum = GridSnap(new Point(task.Symbol.LocationX, task.Symbol.LocationY));
                task.Symbol.Shape = Shape.Create(task.Symbol.Style, task, datum);
            }
        }

        private static SymbolObj CloneSymbol(SymbolObj symbol)
        {
            return new SymbolObj
            {
                Name = symbol.Name,
                Label = symbol.Label,
                Style = symbol.Style,
                LocationX = symbol.LocationX,
                LocationY = symbol.LocationY
            };
        }

        public Visible HitTest(Point p)
        {
            var selection =
                Model.Workflow.Tasks.Select(task => task.Symbol.Shape.HitTest(p)).LastOrDefault(o => o != null);

            // Always reselect the same connector in case of overlap 
            if(Selection.Single is Connector && selection is Connector)
                selection = ((Connector)Selection.Single).HitTest(p);

            return selection ?? Connector.Connectors.Where(c => c.Routed).Select(c => c.HitTest(p)).FirstOrDefault(o => o != null);
        }

        public static Rectangle RoundOutToGrid(Rectangle r)
        {
            r.Y = r.Y - r.Y % GRID_SIZE;
            r.X = r.X - r.X % GRID_SIZE;
            r.Height = (r.Height + GRID_SIZE - 1) / GRID_SIZE * GRID_SIZE;
            r.Width = (r.Width + GRID_SIZE - 1) / GRID_SIZE * GRID_SIZE;
            return r;
        }

        public static Point GridSnap(Point p)
        {
            return new Point
            {
                X = ((2 * p.X + GRID_SIZE) / (2 * GRID_SIZE)) * GRID_SIZE,
                Y = ((2 * p.Y + GRID_SIZE) / (2 * GRID_SIZE)) * GRID_SIZE
            };
        }

        internal void DeleteSelected()
        {
            foreach(var selected in Selection.Items)
            {
                var rect = Rectangle.Empty;
                if(selected is Connector)
                {
                    var conn = (Connector)selected;
                    conn.Delete();
                    Repaint(conn.Bounds);
                }

                else if(selected is Shape)
                {
                    var shape = (Shape)selected;
                    var task = shape.Task;
                    if(task.Symbol.Name.ToLower() == "start")
                    {
                        MessageBox.Show("Cannot delete start task", "Delete Task", MessageBoxButtons.OK);
                        return;
                    }
                    if(MessageBox.Show("Delete task: " + task.TaskId + "?", "Delete Task", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        rect = shape.Bounds;
                        foreach(var conn in Connector.GetConnections(task).ToArray())
                        {
                            rect = Rectangle.Union(rect, conn.Bounds);
                            conn.Delete();
                        }
                        Model.Workflow.Tasks.Remove(task);
                    }
                    Repaint(rect);
                }
            }
            Selection.Clear();
        }
    }

    public class UpdateGraphicsEventArgs : EventArgs
    {
        public Rectangle DamageRect;
    }
}