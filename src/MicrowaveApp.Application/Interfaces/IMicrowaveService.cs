using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Application.Interfaces;

public interface IMicrowaveService
{
    Task<HeatingSessionResponse> QuickStartAsync(CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> StartAsync(StartHeatingRequest request, CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> StartProgramAsync(int programId, CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> AddTimeAsync(CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> PauseAsync(CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse?> PauseOrCancelAsync(CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> ResumeAsync(CancellationToken cancellationToken = default);
    Task<HeatingSessionResponse> AdvanceAsync(int seconds = 1, CancellationToken cancellationToken = default);
    Task CancelAsync(CancellationToken cancellationToken = default);
}
