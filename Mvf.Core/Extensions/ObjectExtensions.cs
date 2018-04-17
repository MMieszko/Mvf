using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace Mvf.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault<T>(this T argument)
        {
            // deal with normal scenarios
            if (argument == null) return true;
            if (object.Equals(argument, default(T))) return true;

            // deal with non-null nullables
            var methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // deal with boxed value types
            var argumentType = argument.GetType();
            if (!argumentType.IsValueType || argumentType == methodType) return false;

            var obj = Activator.CreateInstance(argument.GetType());
            return obj.Equals(argument);
        }

        public static bool DeserializedEquals<T>(this T self, T to)
            where T : class
        {
            return JsonConvert.SerializeObject(self) == JsonConvert.SerializeObject(to);
        }

        public static object ChangeType(this object value, Type desiredType)
        {
            var typeConverter = TypeDescriptor.GetConverter(desiredType);

            object result;

            try
            {
                result = typeConverter.ConvertTo(value, desiredType);
            }
            catch(Exception ex)
            {
                //result = desiredType.GetDefaultValue();
                result = value;
            }

            return result;
        }

        public static bool HasTheSameValuesAs(this object @this, object other)
        {
            var haveSameData = false;

            foreach (var prop in @this.GetType().GetProperties())
            {
                haveSameData = prop.GetValue(@this, null).Equals(prop.GetValue(other, null));

                if (!haveSameData)
                    break;
            }

            return haveSameData;
        }
    }
}
