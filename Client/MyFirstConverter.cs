using FormsMvvm.Converters;

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
