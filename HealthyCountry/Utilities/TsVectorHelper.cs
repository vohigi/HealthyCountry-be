using System;
using System.Text.RegularExpressions;

namespace HealthyCountry.Utilities;

public static class TsVectorHelper
{
    public static string ToTsQueryString(string query)
        => $"{string.Join(":* & ", Rgx.Replace(query,"\\$1").Split(' ', StringSplitOptions.RemoveEmptyEntries))}:*";
        
    private static readonly Regex Rgx = new Regex("(['^$.|?*+()/\\\\#!\"\\\\{\\}\\[\\]\\:\\<\\>\\&])", RegexOptions.Compiled);
}