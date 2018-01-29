using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormsMvvm.Abstract;

namespace FormsMvvm.Locator
{
    public class MvfFormsLocator
    {
        public static List<MvfForm> Forms { get; set; } = new List<MvfForm>();
    }
}
