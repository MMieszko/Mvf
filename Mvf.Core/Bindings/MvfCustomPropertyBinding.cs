using System;

namespace Mvf.Core.Bindings
{
    public abstract class MvfCustomPropertyBinding
    {
        public string ControlPropertyName { get; }
        public Type ControlType { get; }

        public abstract object ReturnValueImplementation(object value, object control);

        protected MvfCustomPropertyBinding(string controlPropertyName, Type controlType)
        {
            ControlPropertyName = controlPropertyName;
            ControlType = controlType;
        }
    }
}
