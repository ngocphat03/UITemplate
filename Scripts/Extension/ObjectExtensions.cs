namespace AXitUnityTemplate.Scripts.Extension
{
    using System;
    using System.Linq;

    public static class ObjectExtensions
    {
        public static bool AreAllPropertiesNullOrDefault(this object obj) => obj == null || obj.GetType().GetProperties().All(p => ObjectExtensions.IsNullOrDefault(p.GetValue(obj)));

        private static bool IsNullOrDefault(object value)
        {
            if (value == null) return true;

            var type = value.GetType();

            return type.IsValueType && value.Equals(Activator.CreateInstance(type));
        }
    }
}