using System.Windows.Forms;
using FormsMvvm.Abstract;
using FormsMvvm.Attributes;

namespace Client
{
    [MvfBindableForm(typeof(FirstViewModel), typeof(Form1))]
    public sealed partial class Form1 : MvfForm
    {
        public Form1()
        {
            InitializeComponent();
            ThisControls = Controls;
        }

        protected override Control.ControlCollection ThisControls { get; set; }
    }
}
