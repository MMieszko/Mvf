using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Mvf.Core.Abstraction;

namespace Mvf.Core.Locator
{
    internal static class MvfLocator
    {
        private static readonly Dictionary<IMvfForm, IMvfViewModel> Pairs;

        static MvfLocator()
        {
            Pairs = new Dictionary<IMvfForm, IMvfViewModel>();
        }

        public static bool HasForm(Form form) => Pairs.ContainsKey(form as IMvfForm ?? throw new InvalidOperationException($"Form must implement {typeof(IMvfForm).Name}"));
        public static bool HasViewModel(IMvfViewModel viewModel) => Pairs.ContainsValue(viewModel);

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

        public static TViewModel CreateViewModel<TViewModel>()
            where TViewModel : IMvfViewModel
        {
            var viewModel = Activator.CreateInstance<TViewModel>();

            return viewModel;

            //var alreadyExisting = Pairs.FirstOrDefault(x => x.Value is TViewModel);

            //if (alreadyExisting != null)
            //{

            // }
        }
    }
}
