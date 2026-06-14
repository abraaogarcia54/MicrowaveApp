using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Application.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldHashPasswordAndPersistUser()
    {
        var users = new FakeUserRepository();
        var service = new AuthService(users, new FakePasswordHasher(), new FakeJwtTokenService());

        var response = await service.RegisterAsync(new LoginRequest("Admin", "123456"));

        response.Username.Should().Be("admin");
        response.Token.Should().Be("token:admin");
        users.Users.Single().PasswordHash.Should().Be("hashed:123456");
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public List<User> Users { get; } = [];

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Users.FirstOrDefault(user => user.Username == username));
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return $"hashed:{password}";
        }

        public bool Verify(string password, string passwordHash)
        {
            return passwordHash == Hash(password);
        }
    }

    private sealed class FakeJwtTokenService : IJwtTokenService
    {
        public string GenerateToken(int userId, string username)
        {
            return $"token:{username}";
        }
    }
}
