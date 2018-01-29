﻿using System;
using System.Collections.Generic;

using Mvf.Core.Abstraction;

namespace Mvf.Core.Locator
{
    internal static class MvfLocator<TViewModel, TForm>
        where TViewModel : IMvfViewModel
        where TForm : IMvfForm
    {
        private static readonly Dictionary<IMvfForm, IMvfViewModel> Pairs;

        static MvfLocator()
        {
            Pairs = new Dictionary<IMvfForm, IMvfViewModel>();
        }

        public static IMvfViewModel GetViewModel(TForm form)
        {
            return Pairs.ContainsKey(form) ? Pairs[form] : default(TViewModel);
        }

        public static TViewModel CreateViewModel(TForm form)
        {
            var vm = Activator.CreateInstance<TViewModel>();
            Pairs.Add(form, vm);
            return vm;
        }

        public static void RemoveViewModel(TForm form)
        {
            Pairs.Remove(form);
        }
    }
}
