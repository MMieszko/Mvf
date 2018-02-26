using System;

namespace Mvf.Core.Bindings
{
    public abstract class MvfCustomPropertyBinding : MvfCustomBinding
    {
        protected MvfCustomPropertyBinding(string controlPropertyName, Type controlType)
            : base(controlPropertyName, controlType)
        {
        }

        public abstract object ReturnValueImplementation(object value, object control);
    }
}
