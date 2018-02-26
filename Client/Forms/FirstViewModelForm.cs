using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModel;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;

namespace Client.Forms
{
    [MvfForm(typeof(FirstViewModel), typeof(FirstViewModelForm))]
    public class FirstViewModelForm : MvfForm<FirstViewModel>
    {
        public FirstViewModelForm()
        {
            
        }
    }
}
