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
        protected IMvfForm Form;
        protected IEnumerable<Control> Controls;
        protected HashSet<PropertyInfo> BindingsHistory;
        protected IEnumerable<PropertyInfo> BindableProperties;
        protected Dictionary<Control, Dictionary<PropertyInfo, object>> LastKnownValues;
        protected CancellationTokenSource ChangesListenerTokenSource;

        private MvfBindingDispatcher()
        {
            this.BindingsHistory = new HashSet<PropertyInfo>();
            this.BindableProperties = new List<PropertyInfo>();
            this.LastKnownValues = new Dictionary<Control, Dictionary<PropertyInfo, object>>();
            this.ChangesListenerTokenSource = new CancellationTokenSource();
        }

        public MvfBindingDispatcher(TViewModel viewModel, IMvfForm instance)
            : this()
        {
            this.ViewModel = viewModel;


            if (!(instance is Form))
            {
                /*Throw exceptoin*/

            }

            Form = instance;
            this.Controls = ((Form)instance).Controls.AsEnumerable();
            this.InitializeAsync();

        }

        private async Task InitializeAsync()
        {
            InitializeBindableProperties();
            await InitStartupBindingsAsync();
            InitialieLastKnownValues();
            StartListeningForChanges();
        }

        private void InitialieLastKnownValues()
        {
            foreach (var control in this.Controls)
            {
                var connectedBindings = this.BindableProperties.Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlName) == control.Name);

                var propertyValuePairs = new Dictionary<PropertyInfo, object>();

                foreach (var controlBinding in connectedBindings)
                {
                    var controlPropetyNameBinding =
                        controlBinding.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlPropertyName);

                    if (controlPropetyNameBinding == null) continue;

                    var controlPropertyValue = control.GetType().GetProperty(controlPropetyNameBinding)?.GetValue(control);

                    propertyValuePairs.Add(controlBinding, controlPropertyValue);
                }

                if (propertyValuePairs.Any())
                    this.LastKnownValues.Add(control, propertyValuePairs);
            }

        }

        private void InitializeBindableProperties()
        {
            this.BindableProperties = ViewModel.GetType()
                .GetProperties()
                // .HavingValues(ViewModel)
                .Where(x => x.HasAttribute<MvfBindable>())
                .ToList()
                .AsReadOnly();
        }

        private async Task InitStartupBindingsAsync()
        {
            foreach (var control in this.Controls)
            {
                var controlBindingProperties = this.BindableProperties
                    .Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlName) == control.Name)
                    .ToList();

                if (!controlBindingProperties.Any()) continue;

                foreach (var bindingProperty in controlBindingProperties)
                    await this.BindControl(control, bindingProperty, bindingProperty.GetMvfConverterType());

            }
        }

        private async void StartListeningForChanges()
        {
            while (true)
            {
                foreach (var control in this.Controls)
                {
                    var connectedBindings = this.BindableProperties.Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlName) == control.Name).ToList();

                    foreach (var controlBinding in connectedBindings)
                    {
                        var controlPropertyname = controlBinding.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlPropertyName);

                        if (string.IsNullOrEmpty(controlPropertyname)) continue;

                        var currentvalue = control.GetProperty(controlPropertyname).GetValue(control);
                        var lastKnownValue = LastKnownValues[control][controlBinding];

                        if (currentvalue.DeserializedEquals(lastKnownValue)) continue;


                        var value = await BindMvfProperty(controlBinding, control.GetProperty(controlPropertyname).GetValue(control),
                            controlBinding.GetMvfConverterType());

                        LastKnownValues[control][controlBinding] = value;
                    }
                }

                try
                {
                    await Task.Delay(1, ChangesListenerTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
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
