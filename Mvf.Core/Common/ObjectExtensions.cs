using System;

namespace Mvf.Core.Common
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault<T>(T argument)
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

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
