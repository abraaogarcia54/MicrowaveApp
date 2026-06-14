using System.Net.Http.Headers;
using System.Net.Http.Json;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Blazor.Services;

public sealed class MicrowaveApiService : IMicrowaveService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthState _authState;

    public MicrowaveApiService(HttpClient httpClient, IAuthState authState)
    {
        _httpClient = httpClient;
        _authState = authState;
    }

    public Task<HeatingSessionResponse> QuickStartAsync(CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>("api/microwave/quick-start", null, cancellationToken);
    }

    public Task<HeatingSessionResponse> StartAsync(StartHeatingRequest request, CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>("api/microwave/start", request, cancellationToken);
    }

    public Task<HeatingSessionResponse> StartProgramAsync(int programId, CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>($"api/microwave/programs/{programId}/start", null, cancellationToken);
    }

    public Task<HeatingSessionResponse> AddTimeAsync(CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>("api/microwave/add-time", null, cancellationToken);
    }

    public Task<HeatingSessionResponse> PauseAsync(CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>("api/microwave/pause", null, cancellationToken);
    }

    public Task<HeatingSessionResponse?> PauseOrCancelAsync(CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse?>("api/microwave/pause-or-cancel", null, cancellationToken);
    }

    public Task<HeatingSessionResponse> ResumeAsync(CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>("api/microwave/resume", null, cancellationToken);
    }

    public Task<HeatingSessionResponse> AdvanceAsync(int seconds = 1, CancellationToken cancellationToken = default)
    {
        return PostAsync<HeatingSessionResponse>($"api/microwave/advance?seconds={seconds}", null, cancellationToken);
    }

    public async Task CancelAsync(CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Post, "api/microwave/cancel");
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ApiErrorReader.ReadMessageAsync(response, cancellationToken));
    }

    private async Task<T> PostAsync<T>(string url, object? body, CancellationToken cancellationToken)
    {
        var request = CreateRequest(HttpMethod.Post, url);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ApiErrorReader.ReadMessageAsync(response, cancellationToken));

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
            response.Content.Headers.ContentLength == 0)
            return default!;

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta inválida da API.");
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string url)
    {
        if (string.IsNullOrWhiteSpace(_authState.Token))
            throw new InvalidOperationException("Autentique-se para executar esta função.");

        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);

        return request;
    }
}
