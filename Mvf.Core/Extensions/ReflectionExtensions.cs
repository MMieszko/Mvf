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

        public static IEnumerable<PropertyInfo> HavingBindableAttribute(this IEnumerable<PropertyInfo> collection)
        {
            var result = collection.Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(MvfBindable) ||
                                                                           a.AttributeType == typeof(MvfConvert)));
            return result;
        }

        public static object GetValue(this PropertyInfo property, object sourceObject)
        {
            var value = property.GetValue(sourceObject, null);
            return value;
        }

        public static string GetControlName(this PropertyInfo property)
        {
            var bindingAttribute = property.GetAttributeOrDefault<MvfBindable>();
            return bindingAttribute?.ControlName;
        }

        public static string GetProprtyName(this PropertyInfo property)
        {
            var bindingAttribute = property.GetAttributeOrDefault<MvfBindable>();
            return bindingAttribute?.PropertyName;
        }

        public static Type GetMfvConverter(this PropertyInfo property)
        {
            var bindingAttribute = property.GetAttributeOrDefault<MvfConvert>();
            return bindingAttribute?.ConverterType;
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
