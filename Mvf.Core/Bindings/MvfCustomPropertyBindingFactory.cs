using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mvf.Core.Bindings
{
    public static class MvfCustomPropertyBindingFactory
    {
        internal static List<MvfCustomPropertyBinding> Bindings;

        static MvfCustomPropertyBindingFactory()
        {
            Bindings = new List<MvfCustomPropertyBinding>();
        }

        public static void Register(MvfCustomPropertyBinding binding)
        {
            Bindings.Add(binding);
        }

        internal static MvfCustomPropertyBinding Find(string controlPropertyName, Control control)
        {
            var result = Bindings.FirstOrDefault(x => control.GetType() == x.ControlType && x.ControlPropertyName == controlPropertyName);

            return result;
        }
    }
}
