using Microsoft.JSInterop;
using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Blazor.Services;

public sealed class AuthState : IAuthState
{
    private const string TokenStorageKey = "microwaveapp.token";
    private const string UsernameStorageKey = "microwaveapp.username";
    private readonly IJSRuntime _jsRuntime;
    private bool _initialized;

    public AuthState(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
    public string? Token { get; private set; }
    public string? Username { get; private set; }
    public event Action? Changed;

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        Token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenStorageKey);
        Username = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UsernameStorageKey);
        _initialized = true;
        Changed?.Invoke();
    }

    public async Task SignInAsync(LoginResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.Token))
            throw new InvalidOperationException("Token de autenticação não retornado pela API.");

        Token = response.Token;
        Username = response.Username;
        _initialized = true;

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenStorageKey, Token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UsernameStorageKey, Username);

        Changed?.Invoke();
    }

    public async Task SignOutAsync()
    {
        Token = null;
        Username = null;
        _initialized = true;

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenStorageKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UsernameStorageKey);

        Changed?.Invoke();
    }
}
