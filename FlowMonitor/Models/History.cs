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

namespace FlowMonitor.Models
{
    [JsonObject]
    public class History
    {
        [JsonProperty(PropertyName = "executionId")]
        public Guid ExecutionId;

        [JsonProperty(PropertyName = "id")]
        public int Id;

        [JsonProperty(PropertyName = "eventType")]
        public string EventType;

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp;

        [JsonProperty(PropertyName = "attributes")]
        public object Attributes;

        public override string ToString()
        {
            return $"{Id,9}  {Timestamp:T}  {MainForm.SplitCamelCase(EventType)}";
        }
    }
}
