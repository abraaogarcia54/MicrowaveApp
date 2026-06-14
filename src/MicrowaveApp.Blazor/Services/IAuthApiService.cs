using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Blazor.Services;

public interface IAuthApiService
{
    Task<LoginResponse> RegisterAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
