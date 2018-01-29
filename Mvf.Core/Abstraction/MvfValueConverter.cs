using System;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfValueConverter
    {
        public abstract object Convert(object value);

        public virtual object ConvertBack(object value) => value;

        public static object GetConvertedValue(Type converter, object value, bool convertBack = false)
        {
            if (converter == null) return value;
            try
            {
                if (!(Activator.CreateInstance(converter) is MvfValueConverter conv)) return value;

                var result = convertBack ? conv.ConvertBack(value) : conv.Convert(value);
                return result;
            }
            catch
            {
                return value;
            }
        }
    }
}
