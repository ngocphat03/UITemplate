namespace UITemplate.Scripts.Extension
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScreenInfoAttribute : Attribute
    {
        public string AddressableScreenPath { get; }

        public ScreenInfoAttribute(string addressableScreenPath) { this.AddressableScreenPath = addressableScreenPath; }
    }
}