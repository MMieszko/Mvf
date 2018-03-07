using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Mvf.Core.Attributes;
using Mvf.Core.Bindings;
using Mvf.Core.Commands;
using Mvf.Core.Common;
using Mvf.Core.Extensions;
using Mvf.Core.Locator;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfForm<TViewModel> : Form, IMvfForm
        where TViewModel : IMvfViewModel
    {
        protected TViewModel ViewModel;
        protected MvfBindingDispatcher<TViewModel> BindingDispatcher { get; private set; }
        protected MvfCommandDispatcher<TViewModel> CommandDispatcher { get; private set; }
        protected event EventHandler<TViewModel> ViewModelSet;

        protected MvfForm()
        {
            base.Load += OnViewInitialized;
        }

        protected virtual void OnViewInitialized(object sender, EventArgs eventArgs)
        {
            InitializeViewModel();
            this.BindingDispatcher = new MvfBindingDispatcher<TViewModel>(ViewModel, this);
            this.CommandDispatcher = new MvfCommandDispatcher<TViewModel>(ViewModel);
            this.CommandDispatcher.Initialize(this.Controls.AsEnumerable());
            ViewModel.OnViewInitialized();
        }

        protected virtual void OnViewModelPropertyChanged(object sender, BindingEventArgs e)
        {
            var control = Controls.AsEnumerable().FirstOrDefault(x => x.Name == e.ControlName);

            if (control == null)
                return;
            //TODO: throw new MvfException($"Could not bind {e.Property.Name} property because related control is not found");

            BindingDispatcher.BindControl(control, e.Property, e.Converter, e.Type);
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

            RaiseViewModelSet(ViewModel);
        }
    }
}
