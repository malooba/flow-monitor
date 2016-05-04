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
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Diagram.DiagramModel.PropertyEditors
{
    /// <summary>
    /// JSON input control
    /// Only validates with valid JSON path
    /// Invalid input is rendered in red
    /// </summary>
    public class JsonPathInput : TextBox
    {
        readonly JValue Null = JValue.CreateNull();

        protected override void OnValidating(CancelEventArgs e)
        {
            e.Cancel = !ValidateJsonPath(Text);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            ForeColor = ValidateJsonPath(Text) ? Color.Black : Color.Red;
        }

        private bool ValidateJsonPath(string jpath)
        {
            try
            {
                Null.SelectToken(jpath);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

