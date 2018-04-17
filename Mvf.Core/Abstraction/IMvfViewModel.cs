using System;
using Mvf.Core.Common;

namespace Mvf.Core.Abstraction
{
    public interface IMvfViewModel
    {
        event EventHandler<BindingEventArgs> PropertyChanged;

        void OnViewInitialized();

        void Navigate<TViewModel>() where TViewModel : IMvfViewModel;
    }
}
