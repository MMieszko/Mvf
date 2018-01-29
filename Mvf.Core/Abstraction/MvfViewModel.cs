using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mvf.Core.Common;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfViewModel : IMvfViewModel
    {
        public event EventHandler<BindingEventArgs> PropertyChanged;
        
        protected void RaisePropertyChanged([CallerMemberName] string callerName = "")
        {
            BindingEventArgs bindingEventArgs = null;

            var method = new StackTrace().GetFrame(1).GetMethod();
            var propName = method.Name.Remove(0, 4);
            var property = method.DeclaringType?.GetProperty(propName);

            if (property == null) return;

            var value = property.GetValue(this, null);

            var bindingConverterPair = property.GetBindingAttributes();

            if (bindingConverterPair.Bindable != null)
            {
                bindingEventArgs = new BindingEventArgs(callerName, bindingConverterPair.Bindable.ControlName,
                    value, bindingConverterPair.Bindable.PropertyName, bindingConverterPair.Bindable.Type, bindingConverterPair.Convert?.ConverterType);
            }
            PropertyChanged?.Invoke(this, bindingEventArgs);
        }
    }
}
