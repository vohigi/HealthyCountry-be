using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HealthyCountry.Utilities
{
    public static class EnumExtentions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var memInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            var descriptionAttribute = memInfo == null
                ? default(DescriptionAttribute)
                : memInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descriptionAttribute == null
                ? enumValue.ToString()
                : descriptionAttribute.Description;
        }
    }
}