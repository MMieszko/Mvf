using System;

namespace Mvf.Core.Attributes
{
    public class MvfBindableForm : Attribute
    {
        public Type ViewModelType { get; }
        public Type FormType { get; }

        /// <inheritdoc />
        /// <summary>
        /// Flags forms as bindable one.
        /// </summary>
        /// <param name="viewModel">Type of <see cref="T:FormsMvvm.Abstract.IMvfViewModel" /></param>
        /// <param name="form">Type of <see cref="T:FormsMvvm.Abstract.IMvfForm" /></param>
        public MvfBindableForm(Type viewModel, Type form)
        {
            this.ViewModelType = viewModel;
            this.FormType = form;
        }
    }
}
