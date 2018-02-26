using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Common;
using Mvf.Core.Extensions;

namespace Mvf.Core.Bindings
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

            if (controlProprty == null)
                throw new MvfException($"{bindingProperty.Name} is not known value of {control}");


            control.BeginInvoke(new Action(() =>
            {

                var updated = UpdateWithCustomUpdater(control, bindingProperty, converter);

                if (updated) return;

                var value = GetCustomBindingValue(control, bindingProperty, converter);
                controlProprty.SetValue(control, value);
            }));
        }
        
        public bool UpdateWithCustomUpdater(Control control, PropertyInfo bindingProperty, Type converter = null)
        {
            var givenValue = bindingProperty.GetValue(ViewModel);
            var customPropertyUpdater = MvfCustomPropertyUpdaterFactory.Find(bindingProperty.GetControlPropertyName(), control);

            if (customPropertyUpdater == null) return false;

            customPropertyUpdater.UpdatedControlImplementation(givenValue, control);

            return true;
        }

        public object GetCustomBindingValue(Control control, PropertyInfo bindingProperty, Type converter = null)
        {
            var givenValue = bindingProperty.GetValue(ViewModel);
            var customPropertyBinding = MvfCustomPropertyBindingFactory.Find(bindingProperty.GetControlPropertyName(), control);

            var value = customPropertyBinding != null ? customPropertyBinding.ReturnValueImplementation(givenValue, control) : givenValue;
            
            return converter == null
                ? value
                : MvfValueConverter.GetConvertedValue(converter, value);
        }
    }
}
