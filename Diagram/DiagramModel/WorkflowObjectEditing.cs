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
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace Diagram.DiagramModel
{

    public class VariableCollection : Dictionary<string, VariableObj>, ICustomTypeDescriptor
    {
        public VariableCollection(IDictionary<string, VariableObj> src = null)
        {
            if(src != null)
                foreach(var kvp in src)
                    Add(kvp.Key, kvp.Value.Clone());
        }

        #region ICustomTypeDescriptor Members

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            // Create a new collection object PropertyDescriptorCollection
            var pds = new PropertyDescriptorCollection(null);

            // Iterate the variables
            foreach(var v in this)
            {
                var pd = new VariablePropertyDescriptor(this, v.Key);
                pds.Add(pd);
            }
            return pds;
        }

        #endregion
    }

    internal class VariableCollectionTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    internal class VariableCollectionConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if(destType == typeof(string) && value is VariableCollection)
            {
                return "Workflow Variables";
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    public class VariablePropertyDescriptor : PropertyDescriptor
    {
        private readonly VariableCollection collection;
        private readonly string name;

        public VariablePropertyDescriptor(VariableCollection collection, string name)
            : base(name, null)
        {
            this.collection = collection;
            this.name = name;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType => collection.GetType();

        public override string DisplayName => name;

        public override string Description => collection[name].Description;

        public override object GetValue(object component)
        {
            return collection[name];
        }

        public override bool IsReadOnly => true;

        public override string Name => name;

        public override Type PropertyType => typeof(VariableObj);

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            collection[name] = (VariableObj)value;
        }
    }

    internal class VariableObjConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value, Type destType)
        {
            if(destType == typeof(string) && value is VariableObj)
            {
                var v = (VariableObj)value;
                return v.ValueString;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }


    public class InputCollection : Dictionary<string, InputObj>, ICustomTypeDescriptor
    {
        public InputCollection(IDictionary<string, InputObj> src = null)
        {
            if(src != null)
                foreach(var kvp in src)
                    Add(kvp.Key, kvp.Value.Clone());
        }

        #region ICustomTypeDescriptor Members

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            // Create a new collection object PropertyDescriptorCollection 
            var pds = new PropertyDescriptorCollection(null);

            // Iterate the Inputs
            foreach(var v in this.Where(v => !v.Value.Hidden))
            {
                var pd = new InputPropertyDescriptor(this, v.Key);
                pds.Add(pd);
            }
            return pds;
        }

        #endregion
    }

    internal class InputCollectionTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    internal class InputCollectionConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if(destType == typeof(string) && value is InputCollection)
            {
                return "Task Inputs";
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    public class InputPropertyDescriptor : PropertyDescriptor
    {
        private readonly InputCollection collection;
        private readonly string name;

        public InputPropertyDescriptor(InputCollection collection, string name)
            : base(name, null)
        {
            this.collection = collection;
            this.name = name;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType => collection.GetType();

        public override string DisplayName => name;

        public override string Description => collection[name].Description;

        public override object GetValue(object component)
        {
            InputObj val;
            collection.TryGetValue(name, out val);
            return val;
        }

        public override bool IsReadOnly => true;

        public override string Name => name;

        public override Type PropertyType => typeof(InputObj);

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            collection[name] = (InputObj)value;
        }
    }

    internal class InputObjConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value, Type destType)
        {
            if(destType == typeof(string) && value is InputObj)
            {
                var v = (InputObj)value;
                return v.ValueString;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }


    public class OutputCollection : Dictionary<string, OutputObj>, ICustomTypeDescriptor
    {
        public OutputCollection(IDictionary<string, OutputObj> src = null)
        {
            if(src != null)
                foreach (var kvp in src)
                    Add(kvp.Key, kvp.Value.Clone());
        }

        #region ICustomTypeDescriptor Members

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            // Create a new collection object PropertyDescriptorCollection
            var pds = new PropertyDescriptorCollection(null);

            // Iterate the list of outputs
            foreach(var v in this)
            {
                var pd = new OutputPropertyDescriptor(this, v.Key);
                pds.Add(pd);
            }
            return pds;
        }

        #endregion
    }

    internal class OutputCollectionTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    internal class OutputCollectionConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if(destType == typeof(string) && value is OutputCollection)
            {
                return "Task Outputs";
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    public class OutputPropertyDescriptor : PropertyDescriptor
    {
        private readonly OutputCollection collection;
        private readonly string name;

        public OutputPropertyDescriptor(OutputCollection collection, string name)
            : base(name, null)
        {
            this.collection = collection;
            this.name = name;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType => collection.GetType();

        public override string DisplayName => name;

        public override string Description => collection[name].Description + Environment.NewLine +  "Var: " + collection[name].ValueString;

        public override object GetValue(object component)
        {
            return collection[name];
        }

        public override bool IsReadOnly => true;

        public override string Name => name;

        public override Type PropertyType => typeof(OutputObj);

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            collection[name] = (OutputObj)value;
        }
    }

    internal class OutputObjConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value, Type destType)
        {
            if(destType == typeof(string) && value is OutputObj)
            {
                var v = (OutputObj)value;
                return v.ValueString;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}
