using System.Text;
using System.Text.RegularExpressions;

namespace System;

public static partial class Base64Extensions
{
    public static bool IsBase64String(this string s)
    {
        s = s
            .Trim();

        return (s.Length % 4 == 0) && Base64Regex().IsMatch(s);
    }

    public static string MakeFileSafeBase64Substitutions(this string source)
    {
        return source
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public static string RemoveFileSafeBase64Substitutions(this string source)
    {
        return source
            .Replace('-', '+')
            .Replace('_', '/');
    }

    public static string EnsureBase64Length(this string source)
    {
        return source
            .PadRight(source.Length + (4 - source.Length % 4) % 4, '=');
    }

    public static int Base64DecrytedLength(this string source)
    {
        return source
            .Trim(' ', '=') // Remove Padding
            .Length
            .Base64DecrytedLength();
    }
    public static int Base64DecrytedLength(this int encrytedLength)
    {
        return (int)(Math.Floor(3 * (encrytedLength / 4.0)));
    }

    public static int Base64EncrytedLength(this byte[] source)
    {
        return source.Length
            .Base64EncrytedLength();
    }
    public static int Base64EncrytedLength(this string source)
    {
        return source
            .Trim()
            .Length
            .Base64EncrytedLength();
    }
    public static int Base64EncrytedLength(this int decrytedLength)
    {
        return 4 * (int)Math.Ceiling(decrytedLength / 3.0);
    }

    public static Base64 ToBase64(this string plainText, Encoding encoding)
    {
        var plainBytes = encoding.GetBytes(plainText);
        return new Base64(Convert.ToBase64String(plainBytes));
    }

    public static string ToPlainText(this Base64 base64, Encoding encoding)
    {
        var plainBytes = base64.ByteValue;
        return encoding.GetString(plainBytes);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)]
    private static partial Regex Base64Regex();
}
