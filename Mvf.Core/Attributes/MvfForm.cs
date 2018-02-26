using System;

namespace Mvf.Core.Attributes
{
    public class MvfForm : Attribute
    {
        public Type ViewModelType { get; }
        public Type FormType { get; }
        
        public MvfForm(Type viewModel, Type form)
        {
            this.ViewModelType = viewModel;
            this.FormType = form;
        }
    }
}
