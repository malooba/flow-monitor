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
using System.Linq;
using System.Windows.Forms;

namespace Diagram.DiagramModel.PropertyEditors
{
    public sealed partial class IdentifierForm<T> : Form
    {
        public string Identifier { get; private set; }
        private readonly Dictionary<string, T>.KeyCollection keys;
        private readonly string item;

        public IdentifierForm(string item, Dictionary<string, T>.KeyCollection keys)
        {
            InitializeComponent();
            Text = $"New {item} Form";
            lblName.Text = $"New {item} Name";
            this.keys = keys;
            this.item = item;
        }

        private void NameChanged(object sender, EventArgs e)
        {
            Identifier = txtName.Text;
        }

        private void OkClick(object sender, EventArgs e)
        {
            if(!Valid())
                DialogResult = DialogResult.None;
        }

        private bool Valid()
        {
            if(string.IsNullOrWhiteSpace(Identifier))
            {
                MessageBox.Show($"Invalid {item} name");
                return false;
            }

            if(keys.Contains(Identifier))
            {
                MessageBox.Show($"Duplicate {item} name");
                return false;
            }

            return true;
        }
    }
}
