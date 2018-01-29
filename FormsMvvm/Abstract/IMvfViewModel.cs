using System;
using FormsMvvm.Common;

namespace FormsMvvm.Abstract
{
    public interface IMvfViewModel
    {
        event EventHandler<BindingEventArgs> PropertyChanged;
    }
}
