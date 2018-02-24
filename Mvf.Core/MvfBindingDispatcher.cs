using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Common;
using Mvf.Core.Extensions;

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

        public void Bind(Control control, PropertyInfo bindingProperty, BindingType? type = null)
        {
            if (type != null && !CanBind(type.Value, bindingProperty)) return;

            var controlProprty = control.GetProperty(bindingProperty.GetProprtyName());
            var converter = bindingProperty.GetMfvConverter();

            if (controlProprty == null) throw new MvfException($"{bindingProperty.Name} is not known value of {control}");

            control.BeginInvoke(new Action(() =>
            {
                if (converter == null)
                    controlProprty.SetValue(control, bindingProperty.GetValue(ViewModel));
                else
                    controlProprty.SetValue(control, MvfValueConverter.GetConvertedValue(converter, bindingProperty.GetValue(ViewModel)), null);
            }));
        }
    }
}
