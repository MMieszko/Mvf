using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvf.Core.Extensions
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static TDesiredType ChangeType<TDesiredType>(this object value)
        {
            TDesiredType result;

            var typeConverter = TypeDescriptor.GetConverter(typeof(TDesiredType));

            try
            {
                result = (TDesiredType)typeConverter.ConvertTo(value, typeof(TDesiredType));
            }
            catch
            {
                result = default(TDesiredType);
            }

            return result;
        }
    }
}
