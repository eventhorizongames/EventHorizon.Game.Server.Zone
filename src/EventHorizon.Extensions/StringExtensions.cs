using System.Text;

public static class StringExtensions
{
    public static byte[] ToBytes(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new byte[0];
        }
        return Encoding.UTF8.GetBytes(
            text
        );
    }
    public static string UppercaseFirstChar(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        char[] a = text.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
    public static string LowercaseFirstChar(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        char[] a = text.ToCharArray();
        a[0] = char.ToLower(a[0]);
        return new string(a);
    }
}
