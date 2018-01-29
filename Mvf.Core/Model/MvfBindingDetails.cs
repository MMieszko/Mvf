using Mvf.Core.Attributes;

namespace Mvf.Core.Model
{
    public class MvfBindingDetails
    {
        public MvfBindable Bindable { get; set; }
        public MvfConvert Convert { get; set; }
        public object Value { get; set; }
        public string CallerName { get; set; }

        public MvfBindingDetails(MvfBindable bindale, MvfConvert convert, object value, string callerName)
        {
            this.Bindable = bindale;
            this.Convert = convert;
            this.Value = value;
            this.CallerName = callerName;
        }

        public MvfBindingDetails()
        {

        }
    }
}
