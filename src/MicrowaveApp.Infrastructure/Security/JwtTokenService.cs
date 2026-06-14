using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int userId, string username)
    {
        var key = _configuration["Jwt:Key"];

        var issuer = _configuration["Jwt:Issuer"] ?? "MicrowaveApp";
        var audience = _configuration["Jwt:Audience"] ?? "MicrowaveApp.Client";
        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes)
            ? minutes
            : 60;

        var signingKey = JwtSecurityKeyFactory.Create(key!);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
