using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using FormsMvvm.Attributes;
using FormsMvvm.Attributes.Bindings;
using FormsMvvm.Common;
using FormsMvvm.Exceptions;
using FormsMvvm.Locator;

namespace FormsMvvm.Abstract
{
    public abstract class MvfViewModel
    {
        public event EventHandler<BindingEventArgs> PropertyChanged;

        [Obsolete]
        public IEnumerable<string> Controls
        {
            get
            {
                var result = new List<string>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    var formAttrobite = type.GetCustomAttribute<MvfBindableForm>();

                    if (formAttrobite == null) continue;

                    if (formAttrobite.ViewModelType == this.GetType())
                    {
                        //TODO: form locator
                        var form = MvfFormsLocator.Forms.Single(x => x.GetType() == formAttrobite.FormType);
                        foreach (var item in form.Controls)
                        {
                            var control = item as Control;
                            result.Add(control?.Name);
                        }
                    }
                }
                return result;
            }

        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string callerName = "")
        {
            BindingEventArgs bindingEventArgs = null;

            var method = new StackTrace().GetFrame(1).GetMethod();
            var propName = method.Name.Remove(0, 4);
            var property = method?.DeclaringType?.GetProperty(propName);

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
