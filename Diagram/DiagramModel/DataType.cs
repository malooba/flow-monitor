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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace Diagram.DiagramModel
{
    public struct DataType
    {
        /// <summary>
        /// Accepts any valid Json value.  String values must be delimited and escaped
        /// </summary>
        public static DataType Any = new DataType("any", ParseAny, GetJsonString);
        /// <summary>
        /// Accepts any Json Object (delimited by {})
        /// </summary>
        public static DataType Object = new DataType("object", ParseObject, GetJsonString);
        /// <summary>
        /// Accepts any Json array (delimited by [])
        /// </summary>
        public static DataType Array = new DataType("array", ParseArray, GetJsonString);
        /// <summary>
        /// Accepts any JSON string.  
        /// </summary>
        public static DataType String = new DataType("string", ParseString, GetUnescapedString);
        /// <summary>
        /// Accepts "true" or "false" (case insensitive)
        /// </summary>
        public static DataType Bool = new DataType("bool", ParseBool, GetJsonString);
        /// <summary>
        /// Accepts any Json integer
        /// </summary>
        public static DataType Integer = new DataType("integer", ParseInteger, GetJsonString);
        /// <summary>
        /// Accepts any Json number
        /// </summary>
        public static DataType Float = new DataType("float", ParseFloat, GetJsonString);
        /// <summary>
        /// Accepts a string that is a valid file or directory path (does not check the path for existence)
        /// </summary>
        public static DataType Path = new DataType("path", ParseFilePath, GetUnescapedString);
        /// <summary>
        /// A bare string literal, implies that the text must be valid JavaScript.
        /// Also signals to the editor that JavaScript syntax highlighting should be used.
        /// </summary>
        public static DataType JavaScript = new DataType("javascript", ParseStringLiteral, GetUnescapedString);
        /// <summary>
        /// Accept a valid Json string.  This will be delimited and escaped as a string.
        /// </summary>
        public static DataType Json = new DataType("json", ParseJson, GetUnescapedString);
        /// <summary>
        /// Accept a valid Xml document.  This will be delimited and escaped as a string.
        /// </summary>
        public static DataType Xml = new DataType("xml", ParseXml, GetUnescapedString);

        /// <summary>
        /// Accept a valid Xml fragment.  This will be delimited and escaped as a string.
        /// </summary>
        public static DataType XmlFragment = new DataType("xmlfragment", ParseXmlFragment, GetUnescapedString);

        /// <summary>
        /// List of all permitted datatypes
        /// </summary>
        public static readonly IList<DataType> AllTypes = new List<DataType>
        {
           Any, Object, Array, String, Bool, Integer, Float, Path, JavaScript, Json, Xml, XmlFragment
        };

        /// <summary>
        /// Validate and parse a string into a JToken
        /// </summary>
        public readonly Func<string, JToken> Parse;

        /// <summary>
        /// Convert a JToken into a string for the editor
        /// </summary>
        public readonly Func<JToken, string> GetEditorString;

        private readonly string name;

        private DataType(string name, Func<string, JToken> parse, Func<JToken, string> getEditorString)
        {
            this.name = name;
            Parse = parse;
            GetEditorString = getEditorString;
        }

        /// <summary>
        /// Convert a datatype into its name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Convert a typename into a DataType
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DataType FromString(string s)
        {
            if(s == null) return Any;
            s = s.ToLower().Trim();
            if(s == string.Empty) return Any;   // Silently default no datatype to Any
            var datatype = AllTypes.FirstOrDefault(dt => dt.name == s);
            if(datatype.name != null) return datatype;
            MessageBox.Show("Invalid datatype - using 'Any'");
            return Any;
        }

        private static JToken ParseAny(string s)
        {
            return JToken.Parse(s);
        }

        private static JToken ParseObject(string s)
        {
            return JObject.Parse(s);
        }

        private static JToken ParseArray(string s)
        {
            return JArray.Parse(s);
        }

        /// <summary>
        /// Parse an escaped and quoted JSON string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static JToken ParseString(string s)
        {
            if(string.IsNullOrEmpty(s))
                return null;
            var j = JToken.Parse(s);
            if(j.Type != JTokenType.String)
                throw new ApplicationException("Invalid JSON string");
            return j;
        }

        /// <summary>
        /// Parse an unquoted, unescaped string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static JToken ParseStringLiteral(string s)
        {
            return s;
        }


        private static JToken ParseJson(string s)
        {
            // Throw if invalid Json
            JToken.Parse(s);
            return s;
        }

        private static JToken ParseBool(string s)
        {
            return bool.Parse(s);
        }

        private static JToken ParseInteger(string s)
        {
            return int.Parse(s);
        }

        private static JToken ParseFloat(string s)
        {
            return double.Parse(s);
        }

        private static JToken ParseFilePath(string s)
        {
            // throw if invalid path string
            System.IO.Path.GetFullPath(s);
            return s;
        }

        private static JToken ParseXml(string s)
        {
            // Throw if invalid Xml
            var settings = new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Document};
            using(var xr = XmlReader.Create(new StringReader(s), settings))
                while(xr.Read()) { }
            return s;
        }

        private static JToken ParseXmlFragment(string s)
        {
            // Throw if invalid Xml
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using(var xr = XmlReader.Create(new StringReader(s), settings))
                while(xr.Read()) { }

            return s;
        }

        private static string GetJsonString(JToken j)
        {
            return j == null ? "" : j.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        private static string GetUnescapedString(JToken j)
        {
            if(j == null) return "";
            return (string)j;
        }
    }
}
