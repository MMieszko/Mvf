using Client.ViewModel;
using Mvf.Core.Attributes;

namespace Client.Forms
{
    [MvfBindableForm(typeof(FirstViewModel), typeof(Form1))]
    public sealed partial class Form1 : FirstViewModelForm
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
}
