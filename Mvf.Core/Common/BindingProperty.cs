using System.Reflection;

namespace Mvf.Core.Common
{
    public class BindngProperty
    {
        public PropertyInfo PropertyInfo { get; }
        public FieldInfo BackingFieldInfo { get; }

        public BindngProperty(PropertyInfo propertyInfo, FieldInfo backingFieldInfo)
        {
            PropertyInfo = propertyInfo;
            BackingFieldInfo = backingFieldInfo;
        }

        public BindngProperty(PropertyInfo propertyInfo, object viewModel)
            : this(propertyInfo, null)
        {
            this.BackingFieldInfo = GetBackingField(viewModel, propertyInfo.Name);
        }

        public static FieldInfo GetBackingField(object obj, string propertyName)
        {


            return obj.GetType().GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
