using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> RegisterAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var username = NormalizeUsername(request.Username);
        var existingUser = await _users.GetByUsernameAsync(username, cancellationToken);

        if (existingUser is not null)
            throw new BusinessException("Usuário já cadastrado.", "USER_ALREADY_EXISTS");

        var user = new User(username, _passwordHasher.Hash(request.Password));
        await _users.AddAsync(user, cancellationToken);

        return new LoginResponse(user.Id, user.Username);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var username = NormalizeUsername(request.Username);
        var user = await _users.GetByUsernameAsync(username, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new BusinessException("Usuário ou senha inválidos.", "INVALID_CREDENTIALS");

        return new LoginResponse(user.Id, user.Username);
    }

    private static string NormalizeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BusinessException("Nome de usuário é obrigatório.", "USERNAME_REQUIRED");

        return username.Trim().ToLowerInvariant();
    }
}
