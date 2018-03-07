using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core.Abstraction;
using Mvf.Core.Bindings;
using Mvf.Core.PropertyUpdaters;
using Ninject;

namespace Mvf.Core
{
    public class MvfAppStart
    {
        private readonly StandardKernel _kernel;

        protected MvfAppStart()
        {
            this._kernel = new StandardKernel();
        }

        protected virtual void RegiesterBindingUpdater()
        {
            MvfCustomPropertyUpdaterFactory.Register(new MvfObservableCollectionBindingUpdater(nameof(ListView.Items), typeof(ListView)));
        }

        public void RunApplication<TForm>(TForm startupForm)
            where TForm : Form
        {
            InitializeIoC(_kernel);
            RegiesterBindingUpdater();
            Application.Run(startupForm);
        }

        protected virtual void InitializeIoC(StandardKernel kernel)
        {

        }
    }
}


