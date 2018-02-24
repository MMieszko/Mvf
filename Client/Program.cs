using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Forms;
using Mvf.Core;

namespace Client
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            MvfCustomPropertyBindingFactory.Register<IMvfObservableCollection>(new MvfCustomPropertyBinding(
                (values, view) =>
                {
                    var listView = view as ListView;
                    listView.Items.Clear();

                    var givenValue = ((IEnumerable)values).GetEnumerator();

                    while (givenValue.MoveNext())
                    {
                        listView.Items.Add(givenValue.Current?.ToString());
                    }

                    return view;
                }));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }
    }
}
