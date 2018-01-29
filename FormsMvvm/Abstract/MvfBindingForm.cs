using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsMvvm.Abstract
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
