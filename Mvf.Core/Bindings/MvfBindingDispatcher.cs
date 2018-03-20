using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;
using Mvf.Core.Common;
using Mvf.Core.Extensions;
using Newtonsoft.Json;

namespace Mvf.Core.Bindings
{
    public class MvfBindingDispatcher<TViewModel>
        where TViewModel : IMvfViewModel
    {
        public TViewModel ViewModel;

        protected HashSet<PropertyInfo> BindingsHistory;

        public MvfBindingDispatcher(TViewModel viewModel)
        {
            this.BindingsHistory = new HashSet<PropertyInfo>();
            this.ViewModel = viewModel;
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

        public async Task<object> BindMvfProperty(PropertyInfo property, object value, Type converter)
        {
            var result = value.ChangeType(property.PropertyType);

            if (converter != null)
                result = MvfValueConverter.GetConvertedBackValue(converter, result);

            await Task.Run(() => property.SetValue(ViewModel, result));

            return value;
        }

        public async Task<bool> BindControl(Control control, PropertyInfo bindingProperty, Type converter = null, BindingType? type = null)
        {
            if (type != null && !CanBind(type.Value, bindingProperty)) return false;

            var controlProprty = control.GetProperty(bindingProperty.GetPropertyFromAttribute<MvfBindable, string>(x => x.ControlPropertyName));

            if (controlProprty == null)
                throw new MvfException($"{bindingProperty.Name} is not known value of {control}");

            return await Task.Factory.FromAsync(Invoke(), (result) => result.IsCompleted);

            IAsyncResult Invoke()
            {
                return control.BeginInvoke(new Action(() =>
                {
                    var value = bindingProperty.GetValue(ViewModel);

                    //TODO ??
                    if (value == null) return;

                    if (UpdateWithCustomUpdater(control, bindingProperty, value, converter)) return;

                    if (converter != null)
                        value = MvfValueConverter.GetConvertedValue(converter, value);

                    controlProprty.SetValue(control, value.ChangeType(controlProprty.PropertyType));

                }));
            }
        }

        public bool UpdateWithCustomUpdater(Control control, PropertyInfo bindingProperty, object value, Type converter = null)
        {
            var givenValue = bindingProperty.GetValue(ViewModel);

            var customPropertyUpdater = GetCustomUpdater(bindingProperty, control);

            if (customPropertyUpdater == null) return false;

            customPropertyUpdater.UpdatedControlImplementation(givenValue, control);

            return true;
        }

        public MvfCustomPropertyUpdater GetCustomUpdater(PropertyInfo bindingProperty, Control control)
        {
            var customPropertyUpdater = MvfCustomPropertyUpdaterFactory.Find(bindingProperty.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlPropertyName), control);

            return customPropertyUpdater;
        }
    }
}
