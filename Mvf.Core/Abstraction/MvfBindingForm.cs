using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core.Attributes;
using Mvf.Core.Bindings;
using Mvf.Core.Commands;
using Mvf.Core.Common;
using Mvf.Core.Converters;
using Mvf.Core.Extensions;
using Mvf.Core.Locator;

namespace Mvf.Core.Abstraction
{
    public class MvfBindingForm<TViewModel> : Form, IMvfForm
        where TViewModel : IMvfViewModel
    {
        protected event EventHandler<TViewModel> ViewModelSet;

        public TViewModel ViewModel { get; private set; }

        protected MvfBindingDispatcher<TViewModel> BindingDispatcher { get; private set; }
        protected MvfCommandDispatcher<TViewModel> CommandDispatcher { get; private set; }
        protected Dictionary<Control, Dictionary<PropertyInfo, object>> LastKnownValues;
        protected List<BindngProperty> BindableProperties;
        protected CancellationTokenSource ChangesListenerTokenSource;

        public MvfBindingForm()
        {
            base.Load += async (s, a) => await OnViewInitialized();
            BindableProperties = new List<BindngProperty>();
            ChangesListenerTokenSource = new CancellationTokenSource();
            LastKnownValues = new Dictionary<Control, Dictionary<PropertyInfo, object>>();
        }

        protected virtual async Task OnViewInitialized()
        {
            InitializeViewModel();
            InitializeBindableProperties();
            InitialieLastKnownValues();

            this.BindingDispatcher = new MvfBindingDispatcher<TViewModel>(ViewModel);
            this.CommandDispatcher = new MvfCommandDispatcher<TViewModel>(ViewModel, this.Controls.AsEnumerable());

            await InitializeStartupBindings();
            StartListeningForChanges();

            ViewModel.OnViewInitialized();
        }

        protected virtual async void OnViewModelPropertyChanged(object sender, BindingEventArgs e)
        {
            var control = Controls.AsEnumerable().FirstOrDefault(x => x.Name == e.ControlName);

            if (control == null)
                return;
            //TODO: throw new MvfException($"Could not bind {e.Property.Name} property because related control is not found");

            await BindingDispatcher.BindControl(control, e.Property, e.Converter, e.Type);
        }

        private void InitializeViewModel()
        {
            var attribute = this.GetType().GetCustomAttribute<MvfForm>();

            if (attribute == null)
                throw new CustomAttributeFormatException($"Could not find {nameof(MvfForm)} attribute over the {this.GetType().Name} form");

            this.ViewModel = MvfLocator.GetViewModel<TViewModel>(this);
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;

            RaiseViewModelSet(ViewModel);
        }

        protected virtual void RaiseViewModelSet(TViewModel e)
        {
            ViewModelSet?.Invoke(this, e);
        }

        protected virtual async Task InitializeStartupBindings()
        {
            foreach (var control in this.Controls.AsEnumerable())
            {
                var controlBindingProperties = this.BindableProperties
                                                   .Select(x => x.PropertyInfo)
                                                   .Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlName) == control.Name)
                                                   .ToList();

                if (!controlBindingProperties.Any()) continue;

                foreach (var bindingProperty in controlBindingProperties)
                    await BindingDispatcher.BindControl(control, bindingProperty, bindingProperty.GetMvfConverterType());

            }
        }

        private async void StartListeningForChanges()
        {
            while (true)
            {
                foreach (var control in this.Controls.AsEnumerable())
                {
                    var connectedBindings = this.BindableProperties.Where(x => x.PropertyInfo.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlName) == control.Name).ToList();

                    foreach (var controlBinding in connectedBindings)
                    {
                        var controlPropertyname = controlBinding.PropertyInfo.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlPropertyName);

                        if (string.IsNullOrEmpty(controlPropertyname)) continue;

                        var currentvalue = control.GetProperty(controlPropertyname).GetValue(control);
                        var lastKnownValue = LastKnownValues[control][controlBinding.PropertyInfo];

                        if (currentvalue.DeserializedEquals(lastKnownValue)) continue;

                        var value = await BindingDispatcher.BindMvfProperty(controlBinding.PropertyInfo, currentvalue, controlBinding.PropertyInfo.GetMvfConverterType());

                        if (controlBinding.PropertyInfo.GetMvfConverterType() != null)
                        {
                            value = MvfValueConverter.GetConvertedValue(controlBinding.PropertyInfo.GetMvfConverterType(),
                                currentvalue);
                        }

                        LastKnownValues[control][controlBinding.PropertyInfo] = value;
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

        private void InitialieLastKnownValues()
        {
            foreach (var control in this.Controls.AsEnumerable())
            {
                var connectedBindings = this.BindableProperties.Select(x => x.PropertyInfo).Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(b => b.ControlName) == control.Name);

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

        protected void InitializeBindableProperties()
        {
            var properties = ViewModel.GetType().GetProperties().Where(x => x.HasAttribute<MvfBindable>()).ToList();
            foreach (var property in properties)
                BindableProperties.Add(new BindngProperty(property, this.ViewModel));
        }
    }
}
