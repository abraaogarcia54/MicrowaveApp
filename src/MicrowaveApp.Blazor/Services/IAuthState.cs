using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Blazor.Services;

public interface IAuthState
{
    bool IsAuthenticated { get; }
    string? Token { get; }
    string? Username { get; }
    event Action? Changed;
    Task InitializeAsync();
    Task SignInAsync(LoginResponse response);
    Task SignOutAsync();
}
