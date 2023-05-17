using System.Linq;

namespace HealthyCountry.Utilities;

public static class StringExtensions
{
    public static string ToLowerCamelcase(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        var chain = value.Split('.');
        return string.Join(".", chain.Select(c=> c.Length == 0 ? string.Empty
            : $"{char.ToLower(c[0])}{(c.Length > 1 ? c[1..] : null)}"));
    }
}