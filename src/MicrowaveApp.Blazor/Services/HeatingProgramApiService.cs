using System.Net.Http.Json;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Blazor.Services;

public sealed class HeatingProgramApiService : IHeatingProgramService
{
    private readonly HttpClient _httpClient;

    public HeatingProgramApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<HeatingProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var programs = await _httpClient.GetFromJsonAsync<HeatingProgramResponse[]>(
            "api/heating-programs",
            cancellationToken);

        return programs ?? [];
    }

    public async Task<HeatingProgramResponse> CreateAsync(CreateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/heating-programs", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await ReadErrorMessageAsync(response, cancellationToken));

        return await response.Content.ReadFromJsonAsync<HeatingProgramResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta inválida ao cadastrar programa.");
    }

    public Task<HeatingProgramResponse> UpdateAsync(int id, UpdateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Alteração de programas customizados será tratada em uma evolução futura.");
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken: cancellationToken);
            return error?.Message ?? "Não foi possível cadastrar o programa.";
        }
        catch
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return string.IsNullOrWhiteSpace(content) ? "Não foi possível cadastrar o programa." : content;
        }
    }

    private sealed record ApiError(string ErrorCode, string Message);
}
