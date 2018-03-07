using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mvf.Core.Extensions
{
    public static class ControlsExtensions
    {
        public static IEnumerable<Control> AsEnumerable(this Control.ControlCollection collection)
        {
            return collection.Cast<Control>().AsEnumerable();
        }
    }
}
