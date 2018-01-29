using System;

namespace Mvf.Core.Common
{
    public class BindingEventArgs : EventArgs
    {
        public BindingType BindingAttribute { get; set; }
        public string ControlName { get; set; }
        public object Value { get; set; }
        public string ViewModelPropertyName { get; set; }
        public Type Converter { get; set; }
        public string CallerName { get; set; }

        public BindingEventArgs(string callerName, string controlName, object value, string propertyName, BindingType type, Type converter)
        {
            this.BindingAttribute = type;
            this.ControlName = controlName;
            this.ViewModelPropertyName = propertyName;
            this.Value = value;
            this.Converter = converter;
        }

        public BindingEventArgs(string callerName, string controlName, object value, string propertyName, BindingType type)
            : this(callerName, controlName, value, propertyName, BindingType.TwoWay, null) { }

        public BindingEventArgs(string callerName, string controlName, object value, string propertyName)
            : this(callerName, controlName, value, propertyName, BindingType.TwoWay) { }
    }
}
