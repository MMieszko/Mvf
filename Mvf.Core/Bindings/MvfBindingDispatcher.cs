using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;
using Mvf.Core.Common;
using Mvf.Core.Extensions;

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
        protected ICollection<RaiseableControl> RaiseableControls;

        private MvfBindingDispatcher()
        {
            this.BindingsHistory = new HashSet<PropertyInfo>();
            this.BindableProperties = new List<PropertyInfo>();
            this.RaiseableControls = new List<RaiseableControl>();
        }

        public MvfBindingDispatcher(TViewModel viewModel, IMvfForm instance)
            : this()
        {
            this.ViewModel = viewModel;
            if (!(instance is Form)) {/*Throw exceptoin*/}
            Form = instance;
            this.Controls = (instance as Form).Controls.AsEnumerable();
            InitializeBindableProperties();
            InitializeStartupBindings();
            StartListeningForChanges(this.Form as Form);
        }

        private void InitializeBindableProperties()
        {
            this.BindableProperties = ViewModel.GetType()
                .GetProperties()
                .HavingValues(ViewModel)
                .Where(x => x.HasAttribute<MvfBindable>())
                .ToList()
                .AsReadOnly();
        }

        private void InitializeStartupBindings()
        {
            foreach (var control in this.Controls)
            {
                var controlBindingProperties = this.BindableProperties
                    .Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlName) == control.Name)
                    .ToList();

                if (!controlBindingProperties.Any()) continue;

                var propetyValuePair = controlBindingProperties.Select(x => (x.GetPropertyFromAttribute<MvfBindable, PropertyInfo>(y => control.GetProperty(y.ControlPropertyName)), x.GetValue(ViewModel))).ToList();

                this.RaiseableControls.Add(new RaiseableControl(control, this.Form as Form, propetyValuePair));

                foreach (var bindingProperty in controlBindingProperties)
                {
                    this.Bind(control, bindingProperty);

                }
            }
        }

        public void StartListeningForChanges(Form form)
        {
            foreach (var control in RaiseableControls)
            {
                control.Run();
                control.ValueChanged += OnControlChanged;
            }
        }

        private void OnControlChanged(object s, PropertyChangedEventArgs e)
        {
            //TODO: Convert back
            Bind(e.Control, e.Property);
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

            var controlProprty = control.GetProperty(bindingProperty.GetPropertyFromAttribute<MvfBindable, string>(x => x.ControlPropertyName));

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
            var customPropertyUpdater = MvfCustomPropertyUpdaterFactory.Find(bindingProperty.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlPropertyName), control);

            if (customPropertyUpdater == null) return false;

            customPropertyUpdater.UpdatedControlImplementation(givenValue, control);

            return true;
        }

        public object GetCustomBindingValue(Control control, PropertyInfo bindingProperty, Type converter = null)
        {
            var givenValue = bindingProperty.GetValue(ViewModel);
            var customPropertyBinding = MvfCustomPropertyBindingFactory.Find(bindingProperty.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlPropertyName), control);

            var value = customPropertyBinding != null ? customPropertyBinding.ReturnValueImplementation(givenValue, control) : givenValue;

            return converter == null
                ? value
                : MvfValueConverter.GetConvertedValue(converter, value);
        }
    }

    public sealed class RaiseableControl
    {
        public Control Control { get; }
        public Form Form { get; }
        public List<(PropertyInfo controlProperty, object value)> NameValue { get; }
        public event EventHandler<PropertyChangedEventArgs> ValueChanged;

        public RaiseableControl(Control control, Form form,
            List<(PropertyInfo controlProperty, object value)> nameValue)
        {
            Form = form;
            Control = control;
            this.NameValue = nameValue;
        }

        private void RaiseValueChanged(PropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        public async void Run()
        {
            foreach (var pair in this.NameValue)
            {
                var currentValue = pair.controlProperty.GetValue(Control);
                 //TODO: IComaprable or smth...
                if (currentValue != pair.value)
                    RaiseValueChanged(new PropertyChangedEventArgs(Control, pair.controlProperty, pair.value, currentValue));
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            Run();
        }
    }

    public class PropertyChangedEventArgs : EventArgs
    {
        public PropertyInfo Property { get; }
        public object OldValue { get; }
        public object NewValue { get; }
        public Control Control { get; }

        public PropertyChangedEventArgs(Control control, PropertyInfo propertyName, object oldValue, object newValue)
        {
            Property = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
            Control = control;
        }
    }
}
