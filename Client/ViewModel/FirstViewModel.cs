using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Mvf.Core;
using Mvf.Core.Abstraction;
using Mvf.Core.Attributes;

namespace Client.ViewModel
{
    public class FirstViewModel : MvfViewModel
    {
        private MvfObserfableCollection<string> _vegetables;
        private MvfObserfableCollection<string> _cartListView;
        private double _bill;
        private Color _billColor;

        public string SelectedVegetable { get; set; }

        [MvfCommandable(nameof(Button.Click), "button1")]
        public ICommand AddToCart { get; set; }

        [MvfCommandable(nameof(ListView.SelectedIndexChanged), "AllVegetablesListView")]
        public ICommand SelectedVegetableChanged { get; set; }

        [MvfCommandable(nameof(Button.Click), "GoToPayButton")]
        public ICommand GoToPayCommand { get; set; }

        [MvfBindable(nameof(ListView.Items), "AllVegetablesListView")]
        public MvfObserfableCollection<string> Vegetables
        {
            get => _vegetables;
            set
            {
                _vegetables = value;
                RaisePropertyChanged();
            }
        }

        [MvfBindable(nameof(ListView.Items), "SelectedVegetablesListView")]
        public MvfObserfableCollection<string> Cart
        {
            get => _cartListView;
            set
            {
                _cartListView = value;
                RaisePropertyChanged();
            }
        }

        [MvfBindable(nameof(Label.Text), "BillLabel")]
        public double Bill
        {
            get { return _bill; }
            set
            {
                _bill = value;
                RaisePropertyChanged();
            }
        }

        [MvfBindable(nameof(Label.ForeColor), "BillLabel")]
        public Color BillColor
        {
            get => _billColor;
            set
            {
                _billColor = value;
                RaisePropertyChanged();
            }
        }

        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
            this.Vegetables = new MvfObserfableCollection<string> { "MARCHEWKA", "ZIEMNIAK", "BURAK", "RZODKIEWKA", "KAPUSTA", "GROCH", "FASOLA", "BRUKSELKA" };
            this.BillColor = Color.Gray;
        }

        public FirstViewModel()
        {
            Vegetables = new MvfObserfableCollection<string>();
            Cart = new MvfObserfableCollection<string>();
            AddToCart = new MvfCommand(OnAddToCart);
            SelectedVegetableChanged = new MvfCommand(OnVegetableChanged);
            GoToPayCommand = new MvfCommand(OnGoToPay);
        }

        private void OnGoToPay(object o)
        {
            throw new NotImplementedException();
        }

        private void OnVegetableChanged(object o)
        {
            var lw = (ListView)o;

            if (lw.SelectedItems.Count == 0) return;

            var selectedIndex = lw.SelectedItems[0].Index;

            this.SelectedVegetable = Vegetables[selectedIndex];
        }

        private void OnAddToCart(object o)
        {
            if (string.IsNullOrEmpty(this.SelectedVegetable))
            {
                MessageBox.Show(@"Nie wybrano wrzywa do dodania");
                return;
            }

            this.Cart.Add(this.SelectedVegetable);
            this.Bill += Math.Round(new Random().NextDouble() * (3.1 - 0.1) + 0.1);

            this.BillColor = this.Bill > 2 ? Color.Red : Color.Green;
        }
    }
}
