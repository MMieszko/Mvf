using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mvf.Core;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;
using Mvf.Core.Common;

namespace Client.ViewModel
{
    public class FirstViewModel : MvfViewModel
    {
        private string _name = "Rysiek";
        private string _surname;
        private int _wiek;
        private MvfObserfableCollection<string> _names;
        /*
        [MvfBindable("ImieTesxtBox", nameof(TextBox.Text), typeof(MyFirstConverter))]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        [MvfBindable("NazwiskoTextBox", nameof(TextBox.Text))]
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                RaisePropertyChanged();
            }
        }
        [MvfBindable("WiekTextbox", nameof(TextBox.Text))]
        public int Wiek
        {
            get => _wiek;
            set
            {
                _wiek = value;
                RaisePropertyChanged();
            }
        }*/
        [MvfBindable(nameof(ListView.Items), "FirstListView")]
        public MvfObserfableCollection<string> Names
        {
            get => _names;
            set
            {
                _names = value;
                RaisePropertyChanged();
            }
        }

        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
            this.Names = new MvfObserfableCollection<string> { "T", "E", "S", "T" };
        }

        public FirstViewModel()
        {
            DoitAsync();
        }

        private async void DoitAsync()
        {
            /*await Task.Delay(TimeSpan.FromSeconds(1));

            //Wiek = 150;

            await Task.Delay(TimeSpan.FromSeconds(1));

            Name = "Elo";

            await Task.Delay(TimeSpan.FromSeconds(3));

            Surname = "Elo2";*/


        }
    }
}
