using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Application.Interfaces;

public interface IHeatingProgramRepository
{
    Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default);
    Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default);
}
