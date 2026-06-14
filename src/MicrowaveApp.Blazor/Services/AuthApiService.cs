using System.Net.Http.Json;
using MicrowaveApp.Application.DTOs;

namespace MicrowaveApp.Blazor.Services;

public sealed class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthState _authState;

    public AuthApiService(HttpClient httpClient, IAuthState authState)
    {
        _httpClient = httpClient;
        _authState = authState;
    }

    public async Task<LoginResponse> RegisterAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync("api/auth/register", request, cancellationToken);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return await AuthenticateAsync("api/auth/login", request, cancellationToken);
    }

    private async Task<LoginResponse> AuthenticateAsync(
        string url,
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ApiErrorReader.ReadMessageAsync(response, cancellationToken));

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta de autenticação inválida.");

        await _authState.SignInAsync(login);
        return login;
    }
}
