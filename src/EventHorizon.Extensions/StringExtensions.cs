using System.Text;

#pragma warning disable CA1050 // Declare types in namespaces
public static class StringExtensions
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static byte[] ToBytes(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return System.Array.Empty<byte>();
        }
        return Encoding.UTF8.GetBytes(
            text
        );
    }

    /// <summary>
    /// This will return a deterministic HashCode for the string.
    /// </summary>
    /// <param name="str">The string a HashCode should be returned for.</param>
    /// <returns>A HashCode unique to this string.</returns>
    public static int GetDeterministicHashCode(
        this string str
    )
    {
        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}
