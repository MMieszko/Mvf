using System;
using System.Reflection;

namespace Mvf.Core.Common
{
    public class BindingEventArgs : EventArgs
    {
        public BindingType Type { get; }
        public string ControlName { get; set; }
        public string ControlPropertyName { get; }
        public object Value { get; }
        public PropertyInfo Property { get; }
        public Type Converter { get; }

        public BindingEventArgs(string controlPropertyName, string controlName, object value, PropertyInfo property, BindingType type, Type converter)
        {
            this.Type = type;
            this.ControlName = controlName;
            this.ControlPropertyName = controlPropertyName;
            this.Property = property;
            this.Value = value;
            this.Converter = converter;
        }

        public BindingEventArgs(string controlPropertyName, string controlName, object value, PropertyInfo property, BindingType type)
            : this(controlPropertyName, controlName, value, property, type, null) { }

        public BindingEventArgs(string controlPropertyName, string controlName, object value, PropertyInfo property)
            : this(controlPropertyName, controlName, value, property, BindingType.TwoWay) { }
    }
}
