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
    public abstract class MvfForm<TViewModel> : MvfBindingForm<TViewModel>
        where TViewModel : IMvfViewModel
    {

    }
}
