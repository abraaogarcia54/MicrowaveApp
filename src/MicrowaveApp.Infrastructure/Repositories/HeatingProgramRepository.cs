using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.Repositories;

public sealed class HeatingProgramRepository : IHeatingProgramRepository
{
    private readonly List<HeatingProgram> _programs = [];

    public Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<HeatingProgram>>(_programs);
    }

    public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_programs.FirstOrDefault(program => program.Id == id));
    }

    public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        _programs.Add(program);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
