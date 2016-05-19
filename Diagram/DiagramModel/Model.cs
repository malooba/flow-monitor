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
using System.Linq;
using System.Windows.Forms;
using Diagram.DiagramView;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remote;

namespace Diagram.DiagramModel
{
    public class Model
    {
        public static WorkflowObj Workflow;


		/// <summary>
		/// Create a model with a new workflow
		/// </summary>
	    public Model()
        {
            Workflow = new WorkflowObj
            {
                Name = Guid.NewGuid().ToString().Substring(0, 8),
                Version = "0.0.0.0",
                ObjType = "workflow",
                TaskList = "decider",
                Tasks = new List<TaskObj>(),
                Variables = new VariableCollection()
            };

            // The single, required start task
            var start = new TaskObj
            {
                TaskId = "start",
                ActivityName = "start",
                ActivityVersion = "0.0.0.0",
                Outflows = new [] { new FlowObj {Name = "Out"} },
                HiddenProperties = new List<string> { "AsyncSignal", "Inputs", "Outputs", "TaskList", "HeartbeatTimeout", "ScheduleToCloseTimeout", "ScheduleToStartTimeout", "StartToCloseTimeout", "TaskPriority" },
                Symbol = new SymbolObj
                {
                    Name = "start",
                    Label = "Start",
                    Style = "circle",
                    LocationX = 100,
                    LocationY = 100
                }
            };
            Workflow.Tasks.Add(start);

		    CreateWorkflowInputVariables(Workflow);
        }

		/// <summary>
		/// Create a model with a workflow from the database
		/// </summary>
		/// <param name="name"></param>
		/// <param name="version"></param>
        public Model(string name, string version)
        {
		    try
		    {
		        var json = RestClient.Get($"/workflows/{name}/versions/{version}");
		        Workflow = JsonConvert.DeserializeObject<WorkflowObj>(json);
                CreateWorkflowInputVariables(Workflow);
            }
		    catch(Exception)
		    {
		        MessageBox.Show("Failed to load Workflow", "Error");
		    }
        }

		/// <summary>
		/// Create a model reading the workflow from a file
		/// </summary>
		/// <param name="path"></param>
        public Model(string path)
        {
            throw new NotImplementedException();
            // TODO:
            // Workflow = WorkflowDefinitions.GetFromFile(path);
            //CreateWorkflowInputVariables(Workflow);
        }

        /// <summary>
        /// Create variables for the workflow input values
        /// </summary>
        /// <param name="wf"></param>
        static void CreateWorkflowInputVariables(WorkflowObj wf)
        {
            // TODO: These should come from site-specific configuration (possible in the app config?)
            var initialVars = new Dictionary<string, string>
            {
                ["_config"] = "object",    // Workflow config
                ["_job"] = "object",       // Job data
                ["_jobType"] = "string",   // Job source/type
                ["_jobId"] = "string"      // External identifier
            };

            foreach(var v in initialVars)
                wf.Variables[v.Key] = new VariableObj {Type = v.Value, Description = "Workflow input variable", Path = $"$.{v.Key}"};
        }

        /// <summary>
        /// Create connection objects for each flow connection in the workflow
        /// </summary>
        public void CreateConnections()
        {
	        foreach (var connection in GetConnections())
	        {
		        var task = connection.Item1;
		        var outflow = connection.Item2;
				var pinFrom = task.Symbol.Shape.pins[outflow.Name];
		        outflow.TargetTask = TaskFromId(outflow.Target);
				var pinTo = outflow.TargetTask.Symbol.Shape.pins[outflow.TargetPin];
				outflow.Connector = Connector.Create(outflow, pinFrom, pinTo);
	        }
        }

        public TaskObj TaskFromId(string id)
        {
            return Workflow.Tasks.Single(t => t.TaskId == id);
        }

        /// <summary>
        /// Get the bounding rectangle of all of the shapes in the diagram
        /// </summary>
        /// <returns></returns>
        public Rectangle GetShapeBounds()
        {
            if(Workflow.Tasks.Count == 0) return Rectangle.Empty;
            return Workflow.Tasks.Skip(1)
                .Aggregate(Workflow.Tasks[0].Symbol.Shape.Bounds, 
                           (current, task) => Rectangle.Union(current, task.Symbol.Shape.Bounds));
        }


        /// <summary>
        /// Get the shape bounds with a margin for routing above and below.
        /// If there is not enough room above than use the space below
        /// Don't add margins left and right because we don't want routes going around the end
        /// </summary>
        /// <param name="margin"></param>
        /// <returns></returns>
        public Rectangle GetDiagramBounds(int margin)
        {
			// Get bounds of shapes
            var shapeBounds = GetShapeBounds();

			// Get bounds of connections
			var connBounds = GetConnections()
				.Where(f => f.Item2 != null && f.Item2.Connector.Bounds != Rectangle.Empty)
				.Select(f => f.Item2.Connector)
				.Aggregate(shapeBounds, (current, conn) => Rectangle.Union(current, conn.Bounds));

			// Ensure required top margin
			var topDeficit = margin - (shapeBounds.Top - connBounds.Top);
			if(topDeficit > 0)
			{
				connBounds.Y -= topDeficit;
				connBounds.Height += topDeficit;
			}

			// Ensure required bottom margin
			var bottomDeficit = margin - (connBounds.Bottom - shapeBounds.Bottom);
			if(bottomDeficit > 0)
			{
				connBounds.Height += bottomDeficit;
			}

			// Push bounds down if we went off the top
			if(connBounds.Y < 0)
				connBounds.Y = 0;

            return connBounds;
        }

        public void RouteConnection(Connector conn)
        {
            var router = new ConnectionRouter(this);
            router.MapConnectors();
            router.RouteConnection(conn);
        }

        public void RouteConnections()
        {
            // TODO: Why is this being recreated?
            var router = new ConnectionRouter(this);
            router.MapConnectors();
            foreach(var connector in Connector.Connectors.Where(c => !c.Routed))
                router.RouteConnection(connector);
        }

        /// <summary>
        /// This methods re-routes a moved tasks connections and any other connections that it now intersects.
        /// It is called when a task is dragged on the diagram.
        /// It is also called when a new task is dropped from the palette. 
        /// In this case there are no existing connections to the task but it may intersect other connections
        /// which will need to be re-routed.
        /// </summary>
        /// <param name="task"></param>
        public void ReRouteTask(TaskObj task)
        {
            var taskConnectors = Connector.GetConnections(task).ToArray();
            var bounds = DiagramView.View.RoundOutToGrid(task.Symbol.Shape.Bounds);
            var otherConnectors = 
                (from c in Connector.Connectors
                 where !taskConnectors.Contains(c)
                 where c.HitsRectangle(bounds)
                 select c).ToList();

            // Need to unroute these first or they interfere with the router heuristics
            foreach(var connector in taskConnectors)
                connector.UnRoute();

            foreach(var connector in otherConnectors)
                connector.UnRoute();

            var router = new ConnectionRouter(this);
            router.MapConnectors();

            foreach(var connector in taskConnectors)
                router.RouteConnection(connector);

            foreach(var connector in otherConnectors)
                router.RouteConnection(connector);
        }

        public FlowObj FindOutflow(TaskObj task, string pinName)
        {
            if(pinName == "Err")
                return task.FailOutflow;

            return task.Outflows.Single(f => f.Name == pinName);
        }

		private IEnumerable<Tuple<TaskObj, FlowObj>> GetConnections()
		{
			foreach(var task in Workflow.Tasks)
			{
				if(task.Outflows != null)
					foreach (var outflow in task.Outflows.Where(outflow => !string.IsNullOrWhiteSpace(outflow.Target)))
						yield return new Tuple<TaskObj, FlowObj>(task, outflow);

				if(!string.IsNullOrWhiteSpace(task.FailOutflow?.Target))
					yield return new Tuple<TaskObj, FlowObj>(task, task.FailOutflow);
			}
		}

	    public bool Save()
	    {
	        try
	        {
	            UpdateLocations();
	            if(UpdateConnections())
	            {
	                var json = JsonConvert.SerializeObject(Workflow);
	                json = RestClient.Put("/workflows", json);
	                var response = JToken.Parse(json);
	                if((string) response == "OK")
	                    return true;

	                MessageBox.Show((string) response, "Save failed");
	            }
	        }
	        catch(Exception ex)
	        {
                MessageBox.Show(ex.Message, "Save failed");
            }
	        return false;
	    }


        public void SaveToFile(string path)
        {
            throw new NotImplementedException();
            // TODO:
            //UpdateLocations();
            //UpdateConnections();
            //WorkflowDefinitions.SaveToFile(path, Workflow);
        }

        private void UpdateLocations()
        {
            foreach(var task in Workflow.Tasks)
            {
                var datum = task.Symbol.Shape.DatumLocation;
                task.Symbol.LocationX = datum.X;
                task.Symbol.LocationY = datum.Y;
            }
        }

        private bool UpdateConnections()
        {
            var ok = true;
            foreach(var task in Workflow.Tasks)
            {
                if(task.Outflows != null)
                {
                    foreach(var outflow in task.Outflows)
                    {
                        if(outflow.Target == null || outflow.TargetPin == null)
                        {
                            MessageBox.Show($"Task {task.TaskId}, Outflow {outflow.Name} is not connected");
                            ok = false;
                            continue;
                        }
                        outflow.Route = outflow.Connector.GetRoute();
                    }
                }
                if(task.FailOutflow?.Connector != null)
                    task.FailOutflow.Route = task.FailOutflow.Connector.GetRoute();
            }
            return ok;
        }
    }
}
