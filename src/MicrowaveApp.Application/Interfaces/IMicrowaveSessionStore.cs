using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Application.Interfaces;

public interface IMicrowaveSessionStore
{
    Task<Microwave> GetOrCreateAsync(string userId, CancellationToken cancellationToken = default);
    Task ClearAsync(string userId, CancellationToken cancellationToken = default);
}
