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
        private readonly List<string> _bindingHistory;

        protected MvfForm()
        {
            _bindingHistory = new List<string>();
            MvfInit();
        }

        private void MvfInit()
        {
            InitializeViewModel();
            InitializeBindings();
            InitializeEvents();
        }

        public virtual void OnLoad()
        {
            //MvfInit();
        }

        public virtual void OnClosed()
        {
            MvfLocator<TViewModel, IMvfForm>.RemoveViewModel(this);
        }

        public virtual void OnViewModelSet()
        {

        }

        private bool CanBind(BindingType type, string viewModelPropertyName)
        {
            switch (type)
            {
                case BindingType.Once when _bindingHistory.Contains(viewModelPropertyName):
                    return false;
                default:
                    return true;
            }
        }

        private void OnBind(BindingEventArgs e)
        {
            if (!CanBind(e.BindingAttribute, e.ViewModelPropertyName)) return;

            var control = Controls.AsEnumerable().First(x => x.Name == e.ControlName);

            control.Invoke(new Action(() =>
            {
                var controlProprty = control.GetProperty(e.ViewModelPropertyName);

                if (controlProprty == null) throw new MvfException($"{e.ViewModelPropertyName} is not know value of {control}");

                try
                {
                    var value = MvfValueConverter.GetConvertedValue(e.Converter, e.Value);
                    controlProprty.SetValue(control, value, null);
                    _bindingHistory.Add(e.ViewModelPropertyName);
                }
                catch (Exception ex)
                {
                    throw new MvfException(ex.Message);
                }
            }));
        }

        #region - Initialize -

        private void InitializeViewModel()
        {
            var attribute = this.GetType().GetCustomAttribute<MvfBindableForm>();

            if (attribute == null)
            {
                //TODO: Logger
                return;
            }

            ViewModel = MvfLocator<TViewModel, IMvfForm>.CreateViewModel(this);
            OnViewModelSet();
        }

        private void InitializeBindings()
        {
            var viewModelProperties = ViewModel.GetType()
                .GetProperties()
                .WhereHaveBindabableAttribute()
                .WhereHaveValues(ViewModel)
                .ToList();

            foreach (Control control in Controls)
            {
                var bindedControls = viewModelProperties.Where(x => x.ControlName() == control.Name).ToList();

                if (!bindedControls.Any()) continue;

                foreach (var binding in bindedControls)
                {
                    control.Invoke(new Action(() =>
                    {
                        var controlProprty = control.GetProperty(binding.ProprtyName());
                        controlProprty.SetValue(control, MvfValueConverter.GetConvertedValue(binding.Converter(), binding.Value(ViewModel), true), null);
                    }));
                }
            }
        }

        private void InitializeEvents()
        {
            this.ViewModel.PropertyChanged += (s, a) => OnBind(a);
            this.Load += (s, a) => OnLoad();
            this.Closed += (s, a) => OnClosed();
        }
        #endregion
    }
}
