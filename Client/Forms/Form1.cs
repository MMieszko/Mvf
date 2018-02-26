using System.Windows.Forms;
using Client.ViewModel;
using Mvf.Core.Attributes;

namespace Client.Forms
{
    [MvfForm(typeof(FirstViewModel), typeof(Form1))]
    public sealed partial class Form1 : FirstViewModelForm
    {
        public Form1()
        {
            InitializeComponent();

            FirstListView.View = View.List;
          
        }
    }
}
