namespace UITemplate.Scripts.Extension
{
    using System;

    public static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this object instance) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(instance.GetType(), typeof(T));
        }
    }
}