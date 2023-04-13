using System.Text;

namespace TodoTests.Tools;

public static class StringTools
{
    public static string GenerateRandomStringOfLength(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTVWXYZabcdefghijklmnopqrstvwxyz123456789";
        
        var result = new StringBuilder(length);
        
        for (var i = 0; i < length; i++) result.Append(chars[random.Next(chars.Length)]);

        return result.ToString();
    }
}