using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Mvf.Core.Attributes;
using Mvf.Core.Bindings;
using Mvf.Core.Common;
using Mvf.Core.Extensions;
using Mvf.Core.Locator;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfForm<TViewModel> : Form, IMvfForm
        where TViewModel : IMvfViewModel
    {
        protected TViewModel ViewModel;
        protected IReadOnlyCollection<PropertyInfo> BindableProperties { get; private set; }
        //protected IReadOnlyCollection<PropertyInfo> CommandableProperties { get; private set; }
        protected MvfBindingDispatcher<TViewModel> BindingDispatcher { get; private set; }
        protected event EventHandler<TViewModel> ViewModelSet;

        protected MvfForm()
        {
            base.Load += OnViewInitialized;
        }

        protected virtual void OnViewInitialized(object sender, EventArgs eventArgs)
        {
            InitializeViewModel();
            InitializeControls();
            InitializeStartupBindings();
            InitializeCommands();
            ViewModel.OnViewInitialized();
        }

        protected virtual void OnViewModelPropertyChanged(object sender, BindingEventArgs e)
        {
            var control = Controls.AsEnumerable().FirstOrDefault(x => x.Name == e.ControlName);

            if (control == null)
                return;
            //throw new MvfException($"Could not bind {e.Property.Name} property because related control is not found");

            BindingDispatcher.Bind(control, e.Property, e.Converter, e.Type);
        }

        protected virtual void RaiseViewModelSet(TViewModel e)
        {
            ViewModelSet?.Invoke(this, e);
        }

        private void InitializeViewModel()
        {
            var attribute = this.GetType().GetCustomAttribute<MvfForm>();

            if (attribute == null)
                throw new CustomAttributeFormatException($"Could not find {nameof(MvfForm)} attribute over the {this.GetType().Name} form");

            this.ViewModel = MvfLocator<TViewModel, IMvfForm>.CreateViewModel(this);
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.BindingDispatcher = new MvfBindingDispatcher<TViewModel>(ViewModel);

            RaiseViewModelSet(ViewModel);
        }

        private void InitializeControls()
        {
            this.BindableProperties = ViewModel.GetType()
                                               .GetProperties()
                                               .HavingValues(ViewModel)
                                               .Where(x => x.HasAttribute<MvfBindable>())
                                               .ToList()
                                               .AsReadOnly();
        }

        private void InitializeCommands()
        {
            var properties = ViewModel.GetType()
                                      .GetProperties()
                                      .Where(x => x.HasAttribute<MvfCommandable>())
                                      .ToList();

            foreach (var commandProperty in properties)
            {
                var controlName = commandProperty.GetPropertyFromAttribute<MvfCommandable, string>(x => x.ControlName);
                var eventName = commandProperty.GetPropertyFromAttribute<MvfCommandable, string>(x => x.EventName);
                var mvfCommand = commandProperty.GetValue(ViewModel, null) as MvfCommand;

                var control = this.Controls.AsEnumerable().FirstOrDefault(x => x.Name == controlName);

                if (control == null || mvfCommand == null) continue;

                var @event = control.GetType().GetEvent(eventName);

                @event.AddEventHandler(control,
                    Delegate.CreateDelegate(@event.EventHandlerType, mvfCommand, mvfCommand.GetType().GetMethod(nameof(MvfCommand.ExecuteImplementation))));

            }
        }

        private void InitializeStartupBindings()
        {
            foreach (Control control in this.Controls)
            {
                var controlBindingProperties = this.BindableProperties
                                                   .Where(x => x.GetPropertyFromAttribute<MvfBindable, string>(y => y.ControlPropertyName) == control.Name)
                                                   .ToList();

                if (!controlBindingProperties.Any()) continue;

                foreach (var bindingProperty in controlBindingProperties)
                    BindingDispatcher.Bind(control, bindingProperty);
            }
        }
    }
}
