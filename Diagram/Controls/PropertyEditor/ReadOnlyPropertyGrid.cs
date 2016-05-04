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
using System.Windows.Forms;

namespace Diagram.Controls.PropertyEditor
{
    public class ReadOnlyPropertyGrid : PropertyGrid
    {
        private bool readOnly;
        public bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = value;
                SetObjectAsReadOnly(SelectedObject, readOnly);
            }
        }

        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            SetObjectAsReadOnly(SelectedObject, readOnly);
            base.OnSelectedObjectsChanged(e);
        }

        private void SetObjectAsReadOnly(object selectedObject, bool isReadOnly)
        {
            if(selectedObject != null)
            {
                TypeDescriptor.AddAttributes(selectedObject, new ReadOnlyAttribute(isReadOnly));
                Refresh();
            }
        }
    }
}
