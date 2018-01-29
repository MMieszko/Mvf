using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using FormsMvvm.Attributes;
using FormsMvvm.Attributes.Bindings;
using FormsMvvm.Common;
using FormsMvvm.Converters;
using FormsMvvm.Exceptions;
using FormsMvvm.Extensions;
using FormsMvvm.Locator;
using FormsMvvm.Model;

namespace FormsMvvm.Abstract
{

    public abstract class MvfForm : Form
    {
        protected abstract Control.ControlCollection ThisControls { get; set; }
        protected MvfViewModel ViewModel;
        private readonly List<string> _bindingHistory;

        protected MvfForm()
        {
            _bindingHistory = new List<string>();
            MvfInit();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.ViewModel.PropertyChanged += (s, a) => OnUiBind(a);
            this.Load += (s, a) => OnLoad();
            this.Closed += (s, a) => OnClosed();
        }

        private void MvfInit()
        {

            void SetViewModel()
            {
                var type = this.GetType();

                var vm = type.GetCustomAttribute<MvfBindableForm>();

                if (vm != null)
                {
                    // TODO: instead of activato check viewmodel locator
                    ViewModel = Activator.CreateInstance(vm.ViewModelType) as MvfViewModel;
                }
            }
            void SetControlValues()
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
                            controlProprty.SetValue(control, GetConvertedValue(binding.Converter(), binding.Value(ViewModel), true), null);
                        }));
                    }
                }
            }

            MvfFormsLocator.Forms.Add(this);

            SetViewModel();
            SetControlValues();

        }

        protected virtual void OnLoad()
        {
            MvfInit();
        }

        protected virtual void OnClosed()
        {

        }

        private void OnUiBind(BindingEventArgs args)
        {
            bool CanBind()
            {
                switch (args.BindingAttribute)
                {
                    case BindingType.Once when _bindingHistory.Contains(args.ViewModelPropertyName):
                        return false;
                    default:
                        return true;
                }
            }

            if (!CanBind()) return;

            var control = Controls.AsEnumerable().First(x => x.Name == args.ControlName);

            control.Invoke(new Action(() =>
            {
                var controlProprty = control.GetProperty(args.ViewModelPropertyName);
                if (controlProprty == null) throw new MvfException($"{args.ViewModelPropertyName} is not know value of {control}");
                try
                {
                    var value = GetConvertedValue(args.Converter, args.Value);
                    controlProprty.SetValue(control, value, null);
                    _bindingHistory.Add(args.ViewModelPropertyName);
                }
                catch (Exception ex)
                {
                    throw new MvfException(ex.Message);
                }
            }));
        }

        private void OnViewModelBind(BindingEventArgs args)
        {

        }
        
        private object GetConvertedValue(Type converter, object value, bool convertBack = false)
        {
            if (converter == null) return value;
            try
            {
                var conv = Activator.CreateInstance(converter) as MvfValueConverter;
                if (conv == null) return value;

                var result = convertBack ? conv.ConvertBack(value) : conv.Convert(value);
                return result;
            }
            catch
            {
                return value;
            }
        }
    }
}
