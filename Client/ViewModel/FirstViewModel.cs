using System;
using System.Threading.Tasks;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;

namespace Client.ViewModel
{
    public class FirstViewModel : MvfViewModel
    {
        private string _name = "Rysiek";
        private string _surname;
        private int _wiek;

        [MvfBindable("ImieTesxtBox", "Text")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        [MvfBindable("NazwiskoTextBox", "Text")]
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                RaisePropertyChanged();
            }
        }
        [MvfBindable("WiekTextbox", "Text")]
        public int Wiek
        {
            get => _wiek;
            set
            {
                _wiek = value;
                RaisePropertyChanged();
            }
        }
         

        public FirstViewModel()
        {
            DoitAsync();
        }

        private async void DoitAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            //Wiek = 150;

            await Task.Delay(TimeSpan.FromSeconds(1));

            Name = "Elo";

            await Task.Delay(TimeSpan.FromSeconds(3));

            Surname = "Elo2";


        }
    }
}
