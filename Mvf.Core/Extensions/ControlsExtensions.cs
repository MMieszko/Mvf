using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mvf.Core.Extensions
{
    public static class ControlsExtensions
    {
        public static Control First(this Control.ControlCollection collection, Predicate<Control> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item as Control))
                {
                    return item as Control;
                }
            }
            return null;
        }

        public static IEnumerable<Control> AsEnumerable(this Control.ControlCollection collection)
        {
            return collection.Cast<Control>().ToList();
        }
    }
}
