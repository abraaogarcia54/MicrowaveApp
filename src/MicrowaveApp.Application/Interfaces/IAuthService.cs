using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
