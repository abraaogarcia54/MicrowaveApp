using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Blazor.Services;

public sealed class EmptyHeatingProgramRepository : IHeatingProgramRepository
{
    public Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<HeatingProgram>>([]);
    }

    public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<HeatingProgram?>(null);
    }

    public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
