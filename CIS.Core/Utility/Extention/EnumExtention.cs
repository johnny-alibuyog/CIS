using System;
using System.ComponentModel;
using System.Linq;

namespace CIS.Core.Utility.Extention;

public static class EnumExtention
{
    public static T AsEnum<T>(this string value) where T : struct
    {
        Enum.TryParse(value, true, out T result);

        return result;
    }

    public static T? AsNullableEnum<T>(this string value) where T : struct
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;


        if (Enum.TryParse(value, true, out T result) == false)
            return null;

        return result;
    }

    public static string GetDescription(this Enum value)
    {
        var attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), true)
            .FirstOrDefault() as DescriptionAttribute;

        return (attribute != null) ? attribute.Description : value.ToString();
    }
}
