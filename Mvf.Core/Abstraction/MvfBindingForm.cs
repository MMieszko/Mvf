using System.Windows.Forms;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfBindingForm : Form
    {
        protected abstract Control.ControlCollection Ctrls { get; set; }
        private MvfViewModel _viewModel;
        
        protected MvfBindingForm(MvfViewModel vm)
        {
            _viewModel = vm;

            _viewModel.PropertyChanged += (s, a) =>
            {
                var btn = Ctrls[0] as Button;

                //btn.Invoke(new Action(() => { btn.Text = a.; }));
            };

        }
    }
}
