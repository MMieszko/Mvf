using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsMvvm.Converters;

namespace FormsMvvm
{
    public static class MvfConverter
    {
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
