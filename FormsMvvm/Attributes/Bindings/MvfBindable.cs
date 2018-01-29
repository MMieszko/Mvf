using System;
using FormsMvvm.Common;

namespace FormsMvvm.Attributes.Bindings
{
    public class MvfBindable : Attribute
    {
        public BindingType Type { get; set; }
        public string ControlName { get; set; }
        public string PropertyName { get; set; }

        public MvfBindable(string controlName, string propertyName, BindingType type)
        {
            this.Type = type;
            this.ControlName = controlName;
            this.PropertyName = propertyName;
        }

        public MvfBindable(string controlName, string propertyName)
            : this(controlName, propertyName, BindingType.TwoWay) { }
    }
}
