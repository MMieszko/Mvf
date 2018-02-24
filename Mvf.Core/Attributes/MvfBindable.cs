using System;
using Mvf.Core.Common;

namespace Mvf.Core.Attributes
{
    public class MvfBindable : Attribute
    {
        public BindingType Type { get; set; }
        public string ControlName { get; set; }
        public string PropertyName { get; set; }
        public Type ConverterType { get; set; }

        public MvfBindable()
        {

        }

        public MvfBindable(string controlName, string propertyName, BindingType type, Type converter)
            : this()
        {
            this.Type = type;
            this.ControlName = controlName;
            this.PropertyName = propertyName;
            this.ConverterType = converter;
        }

        public MvfBindable(string controlName, string propertyName, Type converterType)
            : this(controlName, propertyName, BindingType.TwoWay, converterType) { }

        public MvfBindable(string controlName, string propertyName)
            : this(controlName, propertyName, BindingType.TwoWay, null) { }
    }
}
