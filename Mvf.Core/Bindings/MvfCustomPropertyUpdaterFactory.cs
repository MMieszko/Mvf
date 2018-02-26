using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mvf.Core.Bindings
{
    public class MvfCustomPropertyUpdaterFactory
    {
        internal static List<MvfCustomPropertyUpdater> Bindings;

        static MvfCustomPropertyUpdaterFactory()
        {
            Bindings = new List<MvfCustomPropertyUpdater>();
        }

        public static void Register(MvfCustomPropertyUpdater binding)
        {
            Bindings.Add(binding);
        }

        internal static MvfCustomPropertyUpdater Find(string controlPropertyName, Control control)
        {
            var result = Bindings.FirstOrDefault(x => control.GetType() == x.ControlType && x.ControlPropertyName == controlPropertyName);

            return result;
        }
    }
}
