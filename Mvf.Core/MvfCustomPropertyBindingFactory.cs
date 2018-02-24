using System;
using System.Windows.Forms;

namespace Mvf.Core
{
    public static class MvfCustomPropertyBindingFactory
    {
        internal static TypeDictionary<MvfCustomPropertyBinding> CustomPropertiesBindings;

        static MvfCustomPropertyBindingFactory()
        {
            CustomPropertiesBindings = new TypeDictionary<MvfCustomPropertyBinding>();
        }

        public static MvfCustomPropertyBinding Get(Type type)
        {
            return CustomPropertiesBindings[type];
        }

        public static void Register<TBindingPropertyType>(MvfCustomPropertyBinding binding)
        {
            CustomPropertiesBindings.Add(typeof(TBindingPropertyType), binding);
        }
    }

    public class MvfCustomPropertyBinding
    {
        public Func<object, Control, Control> Func { get; }

        public MvfCustomPropertyBinding(Func<object, Control, Control> func)
        {
            Func = func;

        }
    }
}
