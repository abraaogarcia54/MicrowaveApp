using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Application.Interfaces;

public interface IHeatingProgramService
{
    Task<IReadOnlyCollection<HeatingProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<HeatingProgramResponse> CreateAsync(CreateHeatingProgramRequest request, CancellationToken cancellationToken = default);
    Task<HeatingProgramResponse> UpdateAsync(int id, UpdateHeatingProgramRequest request, CancellationToken cancellationToken = default);
}
