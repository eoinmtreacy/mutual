using System.Text.RegularExpressions;

namespace Share.Util;

public static class InputSanitizer
{
    
    public static string Clean(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));
        input = input.Trim();
        input = Regex.Replace(input, @"<.*?>", string.Empty);
        return input;
    }
    
}