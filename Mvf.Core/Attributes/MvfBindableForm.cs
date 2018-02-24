using System;

namespace Mvf.Core.Attributes
{
    public class MvfBindableForm : Attribute
    {
        public Type ViewModelType { get; }
        public Type FormType { get; }
        
        public MvfBindableForm(Type viewModel, Type form)
        {
            this.ViewModelType = viewModel;
            this.FormType = form;
        }
    }
}
