using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using Mvf.Core.Bindings;

namespace Mvf.Core.PropertyUpdaters
{
    public class MvfObservableCollectionBindingUpdater : MvfCustomPropertyUpdater
    {
        public MvfObservableCollectionBindingUpdater(string controlPropertyName, Type controlType)
            : base(controlPropertyName, controlType)
        {
        }

        public override object UpdatedControlImplementation(object value, object control)
        {
            var listView = control as ListView;

            if (!(value is IMvfObservableCollection values))
                throw new InvalidCastException($"Could not cast {value} to {nameof(IMvfObservableCollection)} while updating {control.ToString()}");

            values.CollectionChanged += (s, a) => OnCollectionChanged(listView, s, a);

            foreach (var val in values)
            {
                listView.Items.Add(val.ToString());
            }

            return listView;
        }

        private void OnCollectionChanged(ListView listView, object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var val in e.NewItems)
            {
                listView.Items.Add(val.ToString());
            }

        }
    }
}
