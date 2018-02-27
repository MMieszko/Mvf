using System;

namespace Mvf.Core.Attributes
{
    public class MvfCommandable : Attribute
    {
        public string EventName { get; }
        public string ControlName { get; set; }

        public MvfCommandable(string eventName, string controlName)
        {
            EventName = eventName;
            ControlName = controlName;
        }
    }
}
