using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core.Bindings;

namespace Mvf.Core
{
    public static class MvfAppStart
    {
        public static void RegiesterBindingUpdater()
        {
            MvfCustomPropertyUpdaterFactory.Register(new MvfObservableCollectionBindingUpdater(nameof(ListView.Items), typeof(ListView)));
        }
    }


    public class MvfObservableCollectionBindingUpdater : MvfCustomPropertyUpdater
    {
        public MvfObservableCollectionBindingUpdater(string controlPropertyName, Type controlType) 
            : base(controlPropertyName, controlType)
        {
        }


        public override object UpdatedControlImplementation(object value, object control)
        {
            var listView = control as ListView;
            var values = value as IEnumerable;

            foreach (var val in values)
            {
                listView.Items.Add(val.ToString());
            }

            return listView;
        }
    }
}
