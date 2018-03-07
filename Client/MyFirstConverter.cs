using Mvf.Core.Abstraction;

namespace Client
{
    public class MyFirstConverter : MvfValueConverter
    {
        public override object Convert(object value)
        {
            return $"{value}, _C_";
        }

        public override object ConvertBack(object value)
        {
            return value;
            //return $"{value}, _CB_";
        }
    }
}
