using System.Security.Cryptography;
using System.Text;

namespace MicrowaveApp.Infrastructure.Security;

public sealed class ConnectionStringProtector
{
    public string Protect(string connectionString, string key)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string é obrigatória.", nameof(connectionString));

        using var aes = CreateAes(key);
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(connectionString);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(aes.IV.Concat(encryptedBytes).ToArray());
    }

    public string Unprotect(string protectedConnectionString, string key)
    {
        if (string.IsNullOrWhiteSpace(protectedConnectionString))
            throw new ArgumentException("Connection string protegida é obrigatória.", nameof(protectedConnectionString));

        var protectedBytes = Convert.FromBase64String(protectedConnectionString);
        var iv = protectedBytes.Take(16).ToArray();
        var encryptedBytes = protectedBytes.Skip(16).ToArray();

        using var aes = CreateAes(key);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }

    private static Aes CreateAes(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Chave de criptografia é obrigatória.", nameof(key));

        var aes = Aes.Create();
        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        return aes;
    }
}
