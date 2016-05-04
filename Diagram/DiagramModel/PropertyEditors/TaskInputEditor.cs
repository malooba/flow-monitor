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
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Diagram.DiagramModel.PropertyEditors
{
    public class TaskInputEditor : UITypeEditor
    {
        private IWindowsFormsEditorService editorService;
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if(context?.Instance != null)
                return UITypeEditorEditStyle.Modal;

            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if(provider != null)
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if(editorService == null || context == null) return value;

            if(!(value is InputCollection))
                throw new ApplicationException("Invalid use of TaskInputEditor");

            var editedValue = new InputCollection((InputCollection)value);
            var form = new TaskInputEditorForm(editedValue);

            return editorService.ShowDialog(form) == System.Windows.Forms.DialogResult.OK ? editedValue : value;
        }
    }
}
