using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        return Task.FromResult(_users.FirstOrDefault(user => user.Username == normalizedUsername));
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
