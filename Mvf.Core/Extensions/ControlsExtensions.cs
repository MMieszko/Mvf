using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mvf.Core.Extensions
{
    public static class ControlsExtensions
    {
        public static Control FirstOrDefault(this Control.ControlCollection collection, Predicate<Control> predicate)
        {
            return (from object item in collection where predicate(item as Control) select item as Control).FirstOrDefault();
        }

        public static IEnumerable<Control> AsEnumerable(this Control.ControlCollection collection)
        {
            return collection.Cast<Control>().AsEnumerable();
        }
    }
}
