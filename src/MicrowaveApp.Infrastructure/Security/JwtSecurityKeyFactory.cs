using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MicrowaveApp.Infrastructure.Security;

public static class JwtSecurityKeyFactory
{
    private const string SigningKeyId = "microwaveapp-signing-key";

    public static SymmetricSecurityKey Create(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("A chave JWT não foi configurada.");

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        {
            KeyId = SigningKeyId
        };
    }
}
