using Mvf.Core.Converters;

namespace Client.Converters
{
    public class MyFirstConverter : MvfValueConverter
    {
        public override object Convert(object value)
        {
            return $"{value},1";
        }

        public override object ConvertBack(object value)
        {
            return value;
            //return $"{value}, _CB_";
        }
    }
}
