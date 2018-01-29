using System;

namespace FormsMvvm.Attributes
{
    public class MvfConvert : Attribute
    {
        public Type ConverterType { get; set; }

        public MvfConvert(Type converterType)
        {
            this.ConverterType = converterType;
        }
    }
}
