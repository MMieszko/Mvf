using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Mvf.Core.Attributes;
using Mvf.Core.Common;
using Mvf.Core.Extensions;
using Mvf.Core.Locator;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfViewModel : IMvfViewModel
    {
        public event EventHandler<BindingEventArgs> PropertyChanged;

        public virtual void OnViewInitialized()
        {

        }

        public void Navigate<TViewModel>() 
            where TViewModel : IMvfViewModel
        {
            if (typeof(IVolatileView).IsAssignableFrom(typeof(TViewModel)))
            {
                
            }
            else
            {
                MvfLocator.GetFormOrDefault<>()
            }
        }
        
        protected void RaisePropertyChanged([CallerMemberName] string callerName = "")
        {
            var bindingsArgs = GetBindingEventArgs(new StackTrace().GetFrame(1).GetMethod());

            PropertyChanged?.Invoke(this, bindingsArgs);
        }

        public BindingEventArgs GetBindingEventArgs(MethodBase methodBase)
        {
            var property = methodBase.DeclaringType?.GetProperty(methodBase.Name.Remove(0, 4));

            if (property == null)
                throw new MvfException($"Could not bind {methodBase.Name}. {nameof(RaisePropertyChanged)} must be called from property.");

            var bindingAttribute = property.GetAttributeOrDefault<MvfBindable>();

            if (bindingAttribute == null)
                throw new MvfException($"Could not bind {methodBase.Name}. The property does not have {nameof(MvfBindable)} attribute");

            var bindingEventArgs = new BindingEventArgs(bindingAttribute.ControlPropertyName, bindingAttribute.ControlName,
                property.GetValue(this, null), property, bindingAttribute.Type, bindingAttribute.ConverterType);

            return bindingEventArgs;
        }
    }
}
