namespace ClaptonStore.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    internal static class EnumExtension
    {
        internal static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);

            return attr != null ? attr.Name : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            var type = value.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());

            return field?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}