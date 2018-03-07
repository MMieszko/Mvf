using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Mvf.Core.Attributes;

namespace Mvf.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<PropertyInfo> HavingValues(this IEnumerable<PropertyInfo> collection, object sourceObject)
        {
            var result = collection.Where(x =>
            {
                var value = x.GetValue(sourceObject, null);
                if (value == null) return false;
                return !value.Equals(value.GetType().GetDefaultValue());
            });
            return result;
        }

        public static bool HasAttribute<TAttribute>(this PropertyInfo @this)
        {
            var result = @this.CustomAttributes.Any(x => x.AttributeType == typeof(TAttribute));

            return result;
        }

        public static TResult GetPropertyFromAttribute<TAtribute, TResult>(this PropertyInfo property, Func<TAtribute, TResult> selector)
              where TAtribute : Attribute
        {
            var attribute = property.GetAttributeOrDefault<TAtribute>();

            if (attribute == null) return default(TResult);

            var result = selector(attribute);

            return result;

        }

        public static Type GetMvfConverterType(this PropertyInfo @this)
        {
            var result = @this.GetPropertyFromAttribute<MvfBindable, Type>(x => x.ConverterType);

            return result;
        }

        public static PropertyInfo GetProperty(this Control control, string propertyName)
        {
            var controlProprty = control.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return controlProprty;
        }

        public static T GetAttributeOrDefault<T>(this PropertyInfo property) where T : class
        {
            var attriute = property.GetCustomAttributes(typeof(T), false).FirstOrDefault(x => x is T) as T;
            return attriute;
        }
    }
}
