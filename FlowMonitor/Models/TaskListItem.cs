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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlowMonitor.Models
{
    [JsonObject]
    public class TaskListItem
    {
        [JsonProperty(PropertyName = "workflowName")]
        public string WorkflowName
        { get; set; }
        [JsonProperty(PropertyName = "workflowVersion")]
        public string WorkflowVersion
        { get; set; }
        [JsonProperty(PropertyName = "jobId")]
        public string JobId
        { get; set; }
        [JsonProperty(PropertyName = "taskList")]
        public string TaskList
        { get; set; }
        [JsonProperty(PropertyName = "executionId")]
        public Guid ExecutionId
        { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public int Priority
        { get; set; }
        [JsonProperty(PropertyName = "taskScheduledEventId")]
        public long TaskScheduledEventId
        { get; set; }
        [JsonProperty(PropertyName = "taskToken")]
        public Guid TaskToken
        { get; set; }
        [JsonProperty(PropertyName = "taskAlarm")]
        public DateTime TaskAlarm
        { get; set; }
        [JsonProperty(PropertyName = "heartbeatTimeout")]
        public int? HeartbeatTimeout
        { get; set; }
        [JsonProperty(PropertyName = "heartbeatAlarm")]
        public DateTime? HeartbeatAlarm
        { get; set; }
        [JsonProperty(PropertyName = "workerId")]
        public string WorkerId
        { get; set; }
        [JsonProperty(PropertyName = "cancelling")]
        public bool Cancelling
        { get; set; }
        [JsonProperty(PropertyName = "scheduledAt")]
        public DateTime ScheduledAt
        { get; set; }
        [JsonProperty(PropertyName = "startedAt")]
        public DateTime? StartedAt
        { get; set; }
        [JsonProperty(PropertyName = "taskSheduleToCloseTimeout")]
        public int? TaskSheduleToCloseTimeout
        { get; set; }
        [JsonProperty(PropertyName = "taskStartToCloseTimeout")]
        public int? TaskStartToCloseTimeout
        { get; set; }
        [JsonProperty(PropertyName = "progress")]
        public int? Progress
        { get; set; }
        [JsonProperty(PropertyName = "progressMessage")]
        public string ProgressMessage
        { get; set; }
        [JsonProperty(PropertyName = "progressData")]
        public string ProgressData
        { get; set; }
        [JsonProperty(PropertyName = "notificationData"), JsonConverter(typeof(JsonStringConverter))]
        public JObject notificationData
        { get; set; }
        [JsonProperty(PropertyName = "schedulingEvent")]
        public ActivityTaskScheduledEvent SchedulingEvent
        { get; set; }
    }
}
