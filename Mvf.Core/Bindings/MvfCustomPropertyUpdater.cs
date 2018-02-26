using System;

namespace Mvf.Core.Bindings
{
    public abstract class MvfCustomPropertyUpdater : MvfCustomBinding
    {
        protected MvfCustomPropertyUpdater(string controlPropertyName, Type controlType)
            : base(controlPropertyName, controlType)
        {
        }

        public abstract object UpdatedControlImplementation(object value, object control);
    }
}
