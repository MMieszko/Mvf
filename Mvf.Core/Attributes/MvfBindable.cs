using System;
using Mvf.Core.Common;

namespace Mvf.Core.Attributes
{
    public class MvfBindable : Attribute
    {
        public BindingType Type { get; set; }
        public string ControlPropertyName { get; set; }
        public string ControlName { get; set; }
        public Type ConverterType { get; set; }

        public MvfBindable()
        {

        }
        
        public MvfBindable(string controlPropertyName, string controlName, BindingType type, Type converter)
            : this()
        {
            this.Type = type;
            this.ControlPropertyName = controlPropertyName;
            this.ControlName = controlName;
            this.ConverterType = converter;
        }

        public MvfBindable(string controlPropertyName, string controlName, Type converterType)
            : this(controlPropertyName, controlName, BindingType.TwoWay, converterType) { }

        public MvfBindable(string controlPropertyName, string controlName)
            : this(controlPropertyName, controlName, BindingType.TwoWay, null) { }
    }
}
