using System.Text.RegularExpressions;

namespace Share.Util;

public static class InputSanitizer
{
    
    public static string Clean(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        input = input.Trim();
        input = Regex.Replace(input, @"<.*?>", string.Empty);
        return input;
    }
    
}