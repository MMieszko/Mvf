using Client.ViewModel;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;

namespace Client.Forms
{
    [MvfForm(typeof(SummaryViewModel), typeof(SummaryViewModelForm))]
    public class SummaryViewModelForm : MvfForm<SummaryViewModel>
    {
    }
}
