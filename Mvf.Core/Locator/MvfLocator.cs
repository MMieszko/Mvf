using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Mvf.Core.Abstraction;
using Mvf.Core.Extensions;

namespace Mvf.Core.Locator
{
    internal static class MvfLocator
    {
        private static readonly Dictionary<IMvfForm, IMvfViewModel> Pairs;

        static MvfLocator()
        {
            Pairs = new Dictionary<IMvfForm, IMvfViewModel>();
        }

        public static bool HasForm<TViewModel>()
            where TViewModel : IMvfViewModel
        {
            var pair = Pairs.FirstOrDefault(x => x.Value is TViewModel);

            return !pair.IsNullOrDefault();
        }
        public static bool HasForm(Form form) => Pairs.ContainsKey(form as IMvfForm ?? throw new InvalidOperationException($"Form must implement {typeof(IMvfForm).Name}"));
        public static bool HasViewModel(IMvfViewModel viewModel) => Pairs.ContainsValue(viewModel);
        public static bool HasViewModel<TViewModel>()
            where TViewModel : IMvfViewModel
        {
            return !Pairs.FirstOrDefault(x => x.Value is TViewModel).IsNullOrDefault();
        }

        public static TViewModel GetViewModel<TViewModel>(IMvfForm form)
            where TViewModel : IMvfViewModel
        {
            if (Pairs.ContainsKey(form))
                return (TViewModel)Pairs[form];

            throw new Exception($"Could not find coressponded view model for given form {form.GetType().Name}");
        }

        public static Form GetForm(IMvfViewModel viewModel)
        {
            if (Pairs.ContainsValue(viewModel))
                return (Form)Pairs.First(x => x.Value == viewModel).Key;

            throw new Exception($"Could not find coressponded form for given view model {viewModel.GetType().Name}");
        }

        public static Form GetForm<TViewModel>()
            where TViewModel : IMvfViewModel
        {
            return Pairs.First(x => x.Value is TViewModel).Key as Form;
        }

        public static Form CreatePair<TViewModel, TForm>()
            where TViewModel : IMvfViewModel
            where TForm : IMvfForm
        {
            var vm = (IMvfViewModel)Activator.CreateInstance<TViewModel>();
            var form = (IMvfForm)Activator.CreateInstance<TForm>();

            if (Pairs.ContainsKey(form))
                Pairs.Remove(form);

            Pairs.Add(form, vm);
            return form as Form;
        }

        public static Form CreatePair<TViewModel>()
            where TViewModel : IMvfViewModel
        {
            var vm = (IMvfViewModel)Activator.CreateInstance<TViewModel>();
            var formType = GetFormType<TViewModel>();
            var form = (IMvfForm)Activator.CreateInstance(formType);

            if (Pairs.ContainsKey(form))
                Pairs.Remove(form);

            Pairs.Add(form, vm);
            return form as Form;
        }

        public static TViewModel CreateViewModel<TViewModel>(IMvfForm form)
            where TViewModel : IMvfViewModel
        {
            var viewModel = Activator.CreateInstance<TViewModel>();

            if (Pairs.ContainsKey(form))
                Pairs[form] = viewModel;
            else
                Pairs.Add(form, viewModel);

            return viewModel;

        }

        public static Type GetFormType<TViewModel>()
            where TViewModel : IMvfViewModel
        {

            var @interface = typeof(MvfForm<TViewModel>);

            var formType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => @interface.IsAssignableFrom(p));

            if (formType == null) throw new Exception($"Could not find {typeof(TViewModel).Name}");

            return formType.First();
        }
    }
}
