using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Forms;

using Mvf.Core.Abstraction;
using Mvf.Core.Extensions;
using Mvf.Core.Common;

namespace Mvf.Core
{
    public class MvfBindingDispatcher<TViewModel>
        where TViewModel : IMvfViewModel
    {
        public TViewModel ViewModel;
        protected HashSet<PropertyInfo> BindingsHistory { get; }

        public MvfBindingDispatcher(TViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.BindingsHistory = new HashSet<PropertyInfo>();
        }

        public bool CanBind(BindingType type, PropertyInfo property)
        {
            switch (type)
            {
                case BindingType.Once when BindingsHistory.Contains(property):
                    return false;
                default:
                    return true;
            }
        }

        public void Bind(Control control, PropertyInfo bindingProperty, Type converter = null, BindingType? type = null)
        {
            if (type != null && !CanBind(type.Value, bindingProperty)) return;

            var controlProprty = control.GetProperty(bindingProperty.GetProprtyName());

            if (controlProprty == null) throw new MvfException($"{bindingProperty.Name} is not known value of {control}");

            var customPropertyBinding = MvfCustomPropertyBindingFactory.Get(bindingProperty.PropertyType);

            control.BeginInvoke(new Action(() =>
            {
                var givenValue = bindingProperty.GetValue(ViewModel);
                var value = customPropertyBinding != null ? customPropertyBinding.Func.Invoke(givenValue, control) : bindingProperty.GetValue(ViewModel);


                if (value is Control c)
                {
                    control.CopyPropertyValues(c);
                }
                else
                {
                    if (converter == null)
                        controlProprty.SetValue(control, value);
                    else
                        controlProprty.SetValue(control, MvfValueConverter.GetConvertedValue(converter, value), null);
                }
            }));
        }
    }
}
