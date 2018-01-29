using System;
namespace FormsMvvm.Attributes
{
    public class MvfBindableForm : Attribute
    {
        public Type ViewModelType { get; }
        public Type FormType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="form"></param>
        public MvfBindableForm(Type viewModel, Type form)
        {
            this.ViewModelType = viewModel;
            this.FormType = form;
        }
    }
}
