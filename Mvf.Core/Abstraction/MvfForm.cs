using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Mvf.Core.Attributes;
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
            ViewModel.OnViewInitialized();
        }
        
        protected virtual void OnViewModelPropertyChanged(object sender, BindingEventArgs e)
        {
            var control = Controls.AsEnumerable().FirstOrDefault(x => x.Name == e.ControlName);

            if (control == null)
                throw new MvfException($"Could not bind {e.Property.Name} property because related control is not found");

            BindingDispatcher.Bind(control, e.Property, e.Converter, e.Type);
        }
        
        protected virtual void RaiseViewModelSet(TViewModel e)
        {
            ViewModelSet?.Invoke(this, e);
        }

        private void InitializeViewModel()
        {
            var attribute = this.GetType().GetCustomAttribute<MvfBindableForm>();

            if (attribute == null)
                throw new CustomAttributeFormatException($"Could not find {nameof(MvfBindableForm)} attribute over the {this.GetType().Name} form");

            this.ViewModel = MvfLocator<TViewModel, IMvfForm>.CreateViewModel(this);
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.BindingDispatcher = new MvfBindingDispatcher<TViewModel>(ViewModel);

            RaiseViewModelSet(ViewModel);
        }

        private void InitializeControls()
        {
            this.BindableProperties = ViewModel.GetType().GetProperties().HavingBindableAttribute().HavingValues(ViewModel).ToList().AsReadOnly();
        }

        private void InitializeStartupBindings()
        {
            foreach (Control control in this.Controls)
            {
                var controlBindingProperties = this.BindableProperties.Where(x => x.GetControlName() == control.Name).ToList();

                if (!controlBindingProperties.Any()) continue;

                foreach (var bindingProperty in controlBindingProperties)
                    BindingDispatcher.Bind(control, bindingProperty);
            }
        }
    }
}
