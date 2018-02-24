using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mvf.Core.Common
{
    public class BindingEventArgs : EventArgs
    {
        public BindingType Type { get; }
        public string ControlName { get; }
        public object Value { get; }
        public PropertyInfo Property { get; }
        public Type Converter { get; }

        public BindingEventArgs(string controlName, object value, PropertyInfo property, BindingType type, Type converter)
        {
            this.Type = type;
            this.ControlName = controlName;
            this.Property = property;
            this.Value = value;
            this.Converter = converter;
        }

        public BindingEventArgs(string controlName, object value, PropertyInfo property, BindingType type)
            : this(controlName, value, property, type, null) { }

        public BindingEventArgs(string controlName, object value, PropertyInfo property)
            : this(controlName, value, property, BindingType.TwoWay) { }
    }
}
