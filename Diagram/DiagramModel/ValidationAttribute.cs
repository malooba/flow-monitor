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

namespace Diagram.DiagramModel
{
    /// <summary>
    /// Applied to a deserialised JSON property string
    /// This indicates how the string should be validated e.g. as a DateTime
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationAttribute : Attribute
    {
        public string ValidateAs;

        public ValidationAttribute(string validateAs = null)
        {
            ValidateAs = validateAs;
        }
    };
}