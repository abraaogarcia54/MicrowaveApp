using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MicrowaveApp.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly MicrowaveDbContext _dbContext;

    public UserRepository(MicrowaveDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();

        return _dbContext.Users
            .FirstOrDefaultAsync(user => user.Username == normalizedUsername, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
