using System.Text;

// TODO: finish my crypto wrapper library
public static class Hash
{
    public static string SHA256String(string input) => Hash.SHA256(input).ToBase64();
    public static byte[] SHA256(string input) => SHA256(Encoding.UTF8.GetBytes(input));
    public static byte[] SHA256(byte[] input)
    {
        var sha256 = System.Security.Cryptography.SHA256.Create();
        return sha256.ComputeHash(input);
    }
}

public static class Misc
{
    public static byte[] ToBytes(this string str) => Encoding.UTF8.GetBytes(str);
    public static string ToUtf8(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
    public static string ToBase64(this string str) => ToBase64(ToBytes(str));
    public static string ToBase64(this byte[] bytes)
    {
        var base64 = Convert.ToBase64String(bytes);
        return base64;
    }

    public static byte[] Base64ToBytes(this string str)
    {
        var bytes = Convert.FromBase64String(str);
        return bytes;
    }
}