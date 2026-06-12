namespace MicrowaveApp.Infrastructure.Security;

public sealed class JwtTokenService
{
    public string GenerateToken(int userId, string username)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userId}:{username}:{DateTimeOffset.UtcNow:O}"));
    }
}
