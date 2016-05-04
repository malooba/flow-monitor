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
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Diagram.DiagramModel.PropertyEditors
{
    /// <summary>
    /// JSON Properties marked with this attribute are required to have string values which are valid JSON.
    /// These strings will be parsed and the C# property value set to the result.
    /// </summary>
    class JsonObjectConverter : TypeConverter
    {
        public JsonObjectConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string) || base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string && context.Instance is InputObj)
            {
                var type = DataType.FromString(((InputObj)context.Instance).Type);
                return type.Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if(destinationType == typeof(string) && value is JToken && context.Instance is InputObj)
            { 
                var type = DataType.FromString(((InputObj)context.Instance).Type);
                return type.GetEditorString((JToken)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
