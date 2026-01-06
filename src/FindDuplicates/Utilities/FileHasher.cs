using System.Security.Cryptography;

namespace FindDuplicates.Utilities;

public static class FileHasher
{
    public static string ComputeFileHash(string filePath)
    {
        using (var md5 = MD5.Create())
        using (var stream = File.OpenRead(filePath))
        {
            byte[] hashBytes = md5.ComputeHash(stream);
            return Convert.ToHexString(hashBytes);
        }
    }

    public static bool IsSystemFile(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        return (fileInfo.Attributes & FileAttributes.System) != 0 ||
               (fileInfo.Attributes & FileAttributes.Hidden) != 0;
    }
}