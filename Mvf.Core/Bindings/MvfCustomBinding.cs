using System;
namespace Mvf.Core.Bindings
{
    public abstract class MvfCustomBinding
    {
        public string ControlPropertyName { get; }
        public Type ControlType { get; }

        protected MvfCustomBinding(string controlPropertyName, Type controlType)
        {
            ControlPropertyName = controlPropertyName;
            ControlType = controlType;
        }
    }
}
