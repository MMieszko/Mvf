using Mvf.Core.Abstraction;

namespace Client
{
    public class MyFirstConverter : MvfValueConverter
    {
        public override object Convert(object value)
        {
            return $"{value} - EDITED!";
        }
    }
}
