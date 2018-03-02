using System;
using System.Reflection;
using System.Windows.Forms;
using Client.ViewModel;
using Mvf.Core.Attributes;
using Mvf.Core.Extensions;

namespace Client.Forms
{
    [MvfForm(typeof(FirstViewModel), typeof(Form1))]
    public sealed partial class Form1 : FirstViewModelForm
    {
        public Form1()
        {
            InitializeComponent();
            this.Name = " FORM 1 ";
            FirstListView.View = View.List;
        }
    }
}
