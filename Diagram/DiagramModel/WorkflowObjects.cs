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
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Diagram.DiagramModel.PropertyEditors;
using Diagram.DiagramView;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable LocalizableElement

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Diagram.DiagramModel
{
    /// <summary>
    /// Definition of an entire workflow
    /// </summary>
    public class WorkflowObj
    {
        /// <summary>
        /// ObjType is always "workflow"
        /// </summary>
        [JsonProperty(PropertyName = "objtype", Required = Required.Always, Order = 0)]
        [Browsable(false)]
        public string ObjType { get; set; }

        /// <summary>
        /// The workflow name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always, Order = 1)]
        [Category("ID")]
        public string Name { get; set; }

        /// <summary>
        /// The workflow version
        /// </summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always, Order = 2)]
        [Category("ID")]
        public string Version { get; set; }

        /// <summary>
        /// Decider default tasklist
        /// </summary>
        [JsonProperty(PropertyName = "tasklist", DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        [Category("Settings")]
        [DefaultValue("decider")]
        public string TaskList { get; set; }

        /// <summary>
        /// Overall workflow timeout in seconds
        /// Must be parsable as a uint
        /// All workflows timeout after a year regardless of this setting
        /// </summary>
        [JsonProperty(PropertyName = "defaultExecutionStartToCloseTimeout", NullValueHandling = NullValueHandling.Ignore,
            Order = 4)]
        [Category("Settings")]
        [Validation("timeout")]
        public string DefaultExecutionStartToCloseTimeout { get; set; }

        /// <summary>
        /// Default task timeout
        /// Must be parsable as a uint or the string "NONE"
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskStartToCloseTimeout",
            DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        [Category("Settings")]
        [Validation("timeoutOrNone")]
        [DefaultValue("NONE")]
        public string DefaultTaskStartToCloseTimeout { get; set; }

        /// <summary>
        /// Name of JSON schema to validate workflow start data
        /// </summary>
        [JsonProperty(PropertyName = "inputSchema", NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        [Category("Input")]
        public string InputSchema { get; set; }

        /// <summary>
        /// Version of JSON schema to validate workflow start data
        /// </summary>
        [JsonProperty(PropertyName = "inputSchemaVersion", NullValueHandling = NullValueHandling.Ignore, Order = 7)]
        [Category("Input")]
        public string InputSchemaVersion { get; set; }

        /// <summary>
        /// Workflow variables and how they are initialised
        /// </summary>
        [JsonProperty(PropertyName = "variables", NullValueHandling = NullValueHandling.Ignore, Order = 8)]
        [Category("Objects")]
        [TypeConverter(typeof(VariableCollectionConvertor))]
        [Editor(typeof(VariableEditor), typeof(UITypeEditor))]
        public VariableCollection Variables { get; set; }

        /// <summary>
        /// Workflow tasks
        /// </summary>
        [Browsable(false), JsonProperty(PropertyName = "tasks", Required = Required.Always, Order = 9)]
        [Category("Objects")]
        public List<TaskObj> Tasks { get; set; }
    }

    public delegate void TaskChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// One workflow task
    /// </summary>
    public class TaskObj : ICustomTypeDescriptor
    {
        /// <summary>
        /// Task ID (unique in workflow)
        /// </summary>
        [JsonProperty(PropertyName = "taskId", Required = Required.Always, Order = 0)]
        [Category("\tID")]
        [DisplayName("Task ID")]
        public string TaskId
        {
            get { return taskId; }
            set
            {
                if(value != taskId)
                {
                    taskId = value;
                    if(Symbol != null)
                    {
                        Symbol.Shape.Invalidate();
                        TaskChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        private string taskId;

        /// <summary>
        /// Detect TaskId changes so that the diagram can be updated
        /// </summary>
        public event TaskChangedEventHandler TaskChanged;
        /// <summary>
        /// Activity Name
        /// </summary>
        [JsonProperty(PropertyName = "activityName", Required = Required.Always, Order = 1)]
        [Category("\tID")]
        [ReadOnly(true)]
        [DisplayName("Activity Name")]
        public string ActivityName { get; set; }

        /// <summary>
        /// Activity Version
        /// </summary>
        [JsonProperty(PropertyName = "activityVersion", Required = Required.Always, Order = 2)]
        [Category("\tID")]
        [ReadOnly(true)]
        [DisplayName("Activity Version")]
        public string ActivityVersion { get; set; }

        /// <summary>
        /// Asynchronous task signal name
        /// </summary>
        [JsonProperty(PropertyName = "asyncSignal", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        [Category("Async")]
        [DisplayName("Async Signal Name")]
        public string AsyncSignal { get; set; }

        /// <summary>
        /// Inputs to the Task Activity
        /// </summary>
        [JsonProperty(PropertyName = "inputs", NullValueHandling = NullValueHandling.Ignore, Order = 4)]
        [Category("I/O")]
        [TypeConverter(typeof(InputCollectionConvertor))]
        [Editor(typeof(TaskInputEditor), typeof(UITypeEditor))]
        public InputCollection Inputs { get; set; }

        /// <summary>
        /// Outputs from Task Activity
        /// </summary>
        [JsonProperty(PropertyName = "outputs", NullValueHandling = NullValueHandling.Ignore, Order = 5)]
        [Category("I/O")]
        [TypeConverter(typeof(OutputCollectionConvertor))]
        [Editor(typeof(TaskOutputEditor), typeof(UITypeEditor))]
        public OutputCollection Outputs { get; set; }

        /// <summary>
        /// Task outflows
        /// This will be one outflow named "Out" or multiple outflows with Activity specific names
        /// </summary>
        [JsonProperty(PropertyName = "outflows", NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        [Browsable(false)]
        public FlowObj[] Outflows { get; set; }

        /// <summary>
        /// Outflow for Activty failure or timeout
        /// </summary>
        [JsonProperty(PropertyName = "failOutflow", NullValueHandling = NullValueHandling.Ignore, Order = 7)]
        [Browsable(false)]
        public FlowObj FailOutflow { get; set; }

        /// <summary>
        /// Tasklist override
        /// </summary>
        [JsonProperty(PropertyName = "tasklist", NullValueHandling = NullValueHandling.Ignore, Order = 8)]
        [Category("Settings")]
        public string TaskList { get; set; }


        [JsonProperty(PropertyName = "heartbeatTimeout", NullValueHandling = NullValueHandling.Ignore, Order = 9)]
        [Category("Settings")]
        public int? HeartbeatTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter override
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "scheduleToCloseTimeout", NullValueHandling = NullValueHandling.Ignore, Order = 10)]
        [Category("Settings")]
        [DisplayName("Schedule To Close Timeout")]
        [Validation("timeoutOrNone")]
        public string ScheduleToCloseTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter override
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "scheduleToStartTimeout", NullValueHandling = NullValueHandling.Ignore, Order = 11)]
        [Category("Settings")]
        [DisplayName("Schedule To Start Timeout")]
        [Validation("timeoutOrNone")]
        public string ScheduleToStartTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter override
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "startToCloseTimeout", NullValueHandling = NullValueHandling.Ignore, Order = 12)]
        [Category("Settings")]
        [DisplayName("Start To Close Timeout")]
        [Validation("timeoutOrNone")]
        public string StartToCloseTimeout { get; set; }

        /// <summary>
        /// Task priority
        /// Must be parsable as an int or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "taskPriority", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate, Order = 13)]
        [Category("Settings")]
        [DisplayName("Task Priority")]
        [Validation("integer")]
        [DefaultValue("0")]
        public string TaskPriority { get; set; }

        /// <summary>
        /// Internal symbol graphic data
        /// </summary>
        [JsonProperty(PropertyName = "symbol", NullValueHandling = NullValueHandling.Ignore, Order = 14)]
        [Browsable(false)]
        public SymbolObj Symbol { get; set; }

        [JsonProperty(PropertyName = "hiddenProperties", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 15)]
        [Browsable(false)]
        public List<string> HiddenProperties { get; set; }

        // TODO:
        // These somewhat hacky methods are used to define classes of task
        // In time these should be replaced with something more robust but I really
        // don't want to create endless flags.
        public bool HasActivity()
		{
            return Symbol.Name.ToLower() != "start" && Symbol.Name.ToLower() != "end" && Symbol.Name.ToLower() != "cleanup";
        }

		public bool HasInflow()
		{
			return Symbol.Name.ToLower() != "start" && Symbol.Name.ToLower() != "cleanup";
		}

		public bool HasFailOutflow()
		{
			return Symbol.Name.ToLower() != "start" && Symbol.Name.ToLower() != "end" && Symbol.Name.ToLower() != "cleanup";
		}

        #region ICustomTypeDescriptor methods
        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection(null);
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return null;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if(HiddenProperties == null)
                return TypeDescriptor.GetProperties(typeof(TaskObj));

            var descriptors = TypeDescriptor
            .GetProperties(typeof(TaskObj))
            .Cast<PropertyDescriptor>()
            .Where(p => HiddenProperties.All(hp => hp != p.Name))
            .ToArray();

            return new PropertyDescriptorCollection(descriptors);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
#endregion
    }

    /// <summary>
    /// Activity definition
    /// </summary>
    public class ActivityObj
    {
        /// <summary>
        /// ObjType is always "activity"
        /// </summary>
        [JsonProperty(PropertyName = "objtype", Required = Required.Always, Order = 0)]
        public string ObjType { get; set; }

        /// <summary>
        /// Activity name registered with Amazon
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always, Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Activity version registered with Amazon
        /// </summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always, Order = 2)]
        public string Version { get; set; }

        /// <summary>
        /// Activity default tasklist
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskList", Required = Required.Always, Order = 3)]
        public string TaskList { get; set; }

        /// <summary>
        /// Activity timeout parameter default
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskScheduleToCloseTimeout", NullValueHandling = NullValueHandling.Ignore,
            Order = 4)]
        [Validation("timeout")]
        public string DefaultTaskScheduleToCloseTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter default
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskScheduleToStartTimeout", NullValueHandling = NullValueHandling.Ignore,
            Order = 5)]
        [Validation("timeout")]
        public string DefaultTaskScheduleToStartTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter default
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskStartToCloseTimeout", NullValueHandling = NullValueHandling.Ignore,
            Order = 6)]
        [Validation("timeout")]
        public string DefaultTaskStartToCloseTimeout { get; set; }

        /// <summary>
        /// Activity timeout parameter default
        /// Must be parsable as a uint or the string "NONE" or empty to default
        /// </summary>
        [JsonProperty(PropertyName = "defaultTaskHeartbeatTimeout", NullValueHandling = NullValueHandling.Ignore,
            Order = 7)]
        [Validation("timeout")]
        public string DefaultTaskHeartbeatTimeout { get; set; }

        /// <summary>
        /// Activity inputs
        /// </summary>
        [JsonProperty(PropertyName = "inputs", NullValueHandling = NullValueHandling.Ignore, Order = 9)]
        public Dictionary<string, InputObj> Inputs { get; set; }

        /// <summary>
        /// Activity outputs
        /// </summary>
        [JsonProperty(PropertyName = "outputs", NullValueHandling = NullValueHandling.Ignore, Order = 10)]
        public Dictionary<string, OutputObj> Outputs { get; set; }

        /// <summary>
        /// Activity description
        /// </summary>
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore, Order = 11)]
        public string Description { get; set; }
    }

    [TypeConverter(typeof(VariableObjConvertor))]
    //[Editor(typeof(VariableEditor), typeof(UITypeEditor))]
    [JsonObject]
    public class VariableObj
    {
        /// <summary>
        /// The literal JSON value assigned to this variable
        /// </summary>
        [JsonProperty(PropertyName = "lit", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        public string Lit { get; set; }

        /// <summary>
        /// Input data type
        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore, Order = 2)]
        public string Type { get; set; }

        /// <summary>
        /// Optional JSON path to select part of the input data
        /// </summary>
        [JsonProperty(PropertyName = "path", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public string Path { get; set; }

        /// <summary>
        /// If true then this input must be satisfied by the Var and Path settings
        /// </summary>
        [JsonProperty(PropertyName = "required", NullValueHandling = NullValueHandling.Ignore, Order = 4)]
        public bool Required { get; set; }

        /// <summary>
        /// Default value if the Lit and Path settings do not produce a value 
        /// </summary>
        [JsonProperty(PropertyName = "default", NullValueHandling = NullValueHandling.Ignore, Order = 5)]
        public string Default { get; set; }

        /// <summary>
        /// Description of input
        /// </summary>
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        public string Description { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string ValueString
        {
            get
            {
                if(!string.IsNullOrEmpty(Path))
                    return "path: " + Path;
                if(!string.IsNullOrEmpty(Lit))
                    return "literal: " + Lit;
                if(Default != null)
                    return "default:" + Default;
                return ""; // "null";
            }
        }

        public VariableObj Clone()
        {
            return new VariableObj
            {
                Lit = Lit,
                Type = Type,
                Path = Path,
                Required = Required,
                Default = Default,
                Description = Description
            };
        }
    }

    /// <summary>
    /// One input to a task
    /// </summary>
    [TypeConverter(typeof(InputObjConvertor))]
    [JsonObject]
    [ReadOnly(true)]
    public class InputObj
    {
        /// <summary>
        /// The variable assigned to this input
        /// </summary>
        [JsonProperty(PropertyName = "var", NullValueHandling = NullValueHandling.Ignore, Order = 0)]
        [ReadOnly(true)]
        public string Var
        { get; set; }

        /// <summary>
        /// The literal JSON value assigned to this input
        /// </summary>
        [JsonProperty(PropertyName = "lit", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        [TypeConverter(typeof(JsonObjectConverter))]
        [ReadOnly(true)]
        public JToken Lit
        { get; set; }

        /// <summary>
        /// Input data type
        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore, Order = 2)]
        [ReadOnly(true)]
        public string Type
        { get; set; }

        /// <summary>
        /// Optional JSON path to select part of the input variable
        /// </summary>
        [JsonProperty(PropertyName = "path", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        [ReadOnly(true)]
        public string Path
        { get; set; }

        /// <summary>
        /// If true then this input must be satisfied by the Var and Path settings
        /// </summary>
        [JsonProperty(PropertyName = "required", NullValueHandling = NullValueHandling.Ignore, Order = 4)]
        [ReadOnly(true)]
        public bool Required
        { get; set; }

        /// <summary>
        /// Default value if the Var and Path settings do not produce a value 
        /// </summary>
        [JsonProperty(PropertyName = "default", NullValueHandling = NullValueHandling.Ignore, Order = 5)]
        [ReadOnly(true)]
        public JToken Default
        { get; set; }

        /// <summary>
        /// Description of input
        /// </summary>
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        [ReadOnly(true)]
        public string Description
        { get; set; }

        /// <summary>
        /// User defined inputs can be deleted or have their type changed
        /// </summary>
        [JsonProperty(PropertyName = "userDefined", NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        [ReadOnly(true)]
        public bool UserDefined
        { get; set; }

        /// <summary>
        /// Hidden inputs are not shown in the editors
        /// They must be configured in the palette.json entry 
        /// </summary>
        [JsonProperty(PropertyName = "hidden", NullValueHandling = NullValueHandling.Ignore, Order = 7)]
        public bool Hidden
        { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string ValueString
        {
            get
            {
                if(!string.IsNullOrEmpty(Var))
                    return "var: " + Var + (string.IsNullOrEmpty(Path) ? "" : "[" + Path + "]");
                if(Lit != null)
                    return "literal: " + Lit.ToString(Formatting.None);
                if(Default != null)
                    return "default:" + Default;
                return "";  // "null";
            }
        }

        public InputObj Clone()
        {
            return new InputObj
            {
                Var = Var,
                Path = Path,
                Lit = Lit?.DeepClone(),
                Type = Type,
                Default = Default?.DeepClone(),
                Description = Description,
                Required = Required,
                UserDefined = UserDefined,
                Hidden = Hidden
            };
        }
    }

    /// <summary>
    /// One output from a task
    /// </summary>
    [TypeConverter(typeof(OutputObjConvertor))]
    [JsonObject]
    [ReadOnly(true)]
    public class OutputObj
    {
        /// <summary>
        /// Output name to select a property from the Activity output object
        /// </summary>
        [JsonProperty(PropertyName = "var", NullValueHandling = NullValueHandling.Ignore, Order = 0)]
        [ReadOnly(true)]
        public string Var
        { get; set; }

        /// <summary>
        /// Output data type
        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        [ReadOnly(true)]
        public string Type
        { get; set; }

        /// <summary>
        /// Description of output
        /// </summary>
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore, Order = 2)]
        [ReadOnly(true)]
        public string Description
        { get; set; }

        /// <summary>
        /// User defined outputs can be deleted or have their type changed
        /// </summary>
        [JsonProperty(PropertyName = "userDefined", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        [ReadOnly(true)]
        public bool UserDefined
        { get; set; }


        [JsonIgnore]
        [Browsable(false)]
        public string ValueString => Var;

        public OutputObj Clone()
        {
            return new OutputObj
            {
                Var = Var,
                Type = Type,
                Description = Description,
                UserDefined = UserDefined
            };
        }
    }

    /// <summary>
    /// Symbol graphical data
    /// </summary>
    public class SymbolObj
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always, Order = 0)]
        public string Name
        { get; set; }

        /// <summary>
        /// Symbol label
        /// </summary>
        [JsonProperty(PropertyName = "label", Required = Required.Always, Order = 1)]
        public string Label 
        { get; set; }

        /// <summary>
        /// Symbol style
        /// </summary>
        [JsonProperty(PropertyName = "style", Required = Required.Always, Order = 1)]
        public string Style
        { get; set; }

        /// <summary>
        /// Symbol X coordinate
        /// </summary>
        [JsonProperty(PropertyName = "locationX", Order = 2)]
        [DefaultValue(100)]
        public int LocationX
        { get; set; }

        /// <summary>
        /// Symbol Y coordinate
        /// </summary>
        [JsonProperty(PropertyName = "locationY", Order = 3)]
        [DefaultValue(100)]
        public int LocationY
        { get; set; }

        [JsonIgnore]
        public Shape Shape 
        { get; set; }
    }

    public class FlowObj
    {
        /// <summary>
        /// Pin name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always, Order = 0)]
        public string Name
        { get; set; }

	    /// <summary>
	    /// Target name
	    /// </summary>
	    [JsonProperty(PropertyName = "target", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
	    public string Target
	    {
		    get { return TargetTask == null ? target : TargetTask.TaskId; }
		    private set { target = value; }
	    }
		// Temporary storage on load
		// This is nulled once the TargetTask has been configured
	    private string target;

        /// <summary>
        /// Target pin name
        /// </summary>
        [JsonProperty(PropertyName = "targetPin", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        [DefaultValue("In")]
        public string TargetPin
        { get; set; }

        /// <summary>
        /// Target name
        /// </summary>
        [JsonProperty(PropertyName = "route", NullValueHandling = NullValueHandling.Ignore, Order = 1)]
        public int[] Route
        { get; set; }

	    /// <summary>
	    /// Cached target TaskObj
	    /// </summary>
	    [JsonIgnore]
	    public TaskObj TargetTask
	    {
		    get { return targetTask; }
		    set
		    {
			    targetTask = value;
			    Target = null;
		    }
	    }

	    private TaskObj targetTask;

        [JsonIgnore]
        public Connector Connector
        { get; set; }
    }

    public class PaletteObj
    {
        [JsonProperty(PropertyName = "group", Required = Required.Always, Order = 0)]
        public string Group { get; set; }
        [JsonProperty(PropertyName = "image", Required = Required.Always, Order = 1)]
        public string Image { get; set; }
        [JsonProperty(PropertyName = "task", Required = Required.Always, Order = 2)]
        public TaskObj Task { get; set; }
    }

    public enum InputType
    {
        String,
        Integer,
        Float,
        Boolean,
        TimeCode,
        FilePath
    }
}
