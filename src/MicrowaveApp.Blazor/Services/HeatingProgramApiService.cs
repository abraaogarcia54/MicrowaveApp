using System.Net.Http.Headers;
using System.Net.Http.Json;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Blazor.Services;

public sealed class HeatingProgramApiService : IHeatingProgramService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthState _authState;

    public HeatingProgramApiService(HttpClient httpClient, IAuthState authState)
    {
        _httpClient = httpClient;
        _authState = authState;
    }

    public async Task<IReadOnlyCollection<HeatingProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(HttpMethod.Get, "api/heating-programs");
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ApiErrorReader.ReadMessageAsync(response, cancellationToken));

        var programs = await response.Content.ReadFromJsonAsync<HeatingProgramResponse[]>(
            cancellationToken: cancellationToken);

        return programs ?? [];
    }

    public async Task<HeatingProgramResponse> CreateAsync(CreateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        var httpRequest = CreateRequest(HttpMethod.Post, "api/heating-programs");
        httpRequest.Content = JsonContent.Create(request);
        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ApiErrorReader.ReadMessageAsync(response, cancellationToken));

        return await response.Content.ReadFromJsonAsync<HeatingProgramResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta inválida ao cadastrar programa.");
    }

    public Task<HeatingProgramResponse> UpdateAsync(int id, UpdateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Alteração de programas customizados será tratada em uma evolução futura.");
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
