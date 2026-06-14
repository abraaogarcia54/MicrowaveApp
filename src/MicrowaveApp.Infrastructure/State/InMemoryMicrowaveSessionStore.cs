using System.Collections.Concurrent;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.State;

public sealed class InMemoryMicrowaveSessionStore : IMicrowaveSessionStore
{
    private readonly ConcurrentDictionary<string, Microwave> _sessions = new();

    public Task<Microwave> GetOrCreateAsync(string userId, CancellationToken cancellationToken = default)
    {
        var microwave = _sessions.GetOrAdd(userId, _ => new Microwave());
        return Task.FromResult(microwave);
    }

    public Task ClearAsync(string userId, CancellationToken cancellationToken = default)
    {
        _sessions.TryRemove(userId, out _);
        return Task.CompletedTask;
    }
}
