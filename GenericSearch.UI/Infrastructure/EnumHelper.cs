using System.Reflection;

namespace GenericSearch.UI.Infrastructure;

public static class EnumHelper
{
    public static string GetDisplayName(this object enumValue)
    {
        if (enumValue == null)
        {
            throw new ArgumentNullException(nameof(enumValue));
        }

        if (!enumValue.GetType().IsEnum)
        {
            throw new ArgumentException("Please provide enum value.", nameof(enumValue));
        }

        var displayAttribute = enumValue
            .GetType()
            .GetMember(enumValue.ToString()!)
            .First()
            .GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();

        if (displayAttribute == null)
        {
            return enumValue.ToString()!;
        }
        else
        {
            return displayAttribute.GetName() ?? enumValue.ToString()!;
        }
    }
}
