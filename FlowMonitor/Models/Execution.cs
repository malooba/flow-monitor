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

// ReSharper disable UnusedMember.Global

namespace FlowMonitor.Models
{
    [JsonObject]
    public class Execution
    {
        [JsonProperty("executionId")]
        public Guid ExecutionId { get; set; }

        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("workflowName")]
        public string WorkflowName { get; set; }

        [JsonProperty("workflowVersion")]
        [JsonConverter(typeof(VersionConverter))]
        public string WorkflowVersion { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("awaitingDecision")]
        public bool AwaitingDecision { get; set; }

        [JsonProperty("deciderToken")]
        public Guid? DeciderToken { get; set; }

        [JsonProperty("deciderAlarm")]
        public DateTime? DeciderAlarm { get; set; }

        [JsonProperty("decisionList")]
        public string DecisionList { get; set; }

        [JsonProperty("historySeen")]
        public int HistorySeen { get; set; }

        [JsonProperty("lastSeen")]
        public DateTime? LastSeen { get; set; }

        [JsonProperty("executionStartToCloseTimeout")]
        public int? ExecutionStartToCloseTimeout { get; set; }

        [JsonProperty("taskScheduleToCloseTimeout")]
        public int? TaskScheduleToCloseTimeout { get; set; }

        [JsonProperty("taskScheduleToStartTimeout")]
        public int? TaskScheduleToStartTimeout { get; set; }

        [JsonProperty("taskStartToCloseTimeout")]
        public int? TaskStartToCloseTimeout { get; set; }
    }
}