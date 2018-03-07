using System;

namespace Mvf.Core.Abstraction
{
    public abstract class MvfValueConverter
    {
        public abstract object Convert(object value);

        public abstract object ConvertBack(object value);

        public static object GetConvertedValue(Type converter, object value)
        {
            if (converter == null) return value;

            if (!(Activator.CreateInstance(converter) is MvfValueConverter conv)) return value;

            return conv.Convert(value);
        }

        public static object GetConvertedBackValue(Type converter, object value)
        {
            if (converter == null) return value;

            if (!(Activator.CreateInstance(converter) is MvfValueConverter conv)) return value;

            return conv.ConvertBack(value);
        }
    }
}
