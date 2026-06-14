using System.Net.Http.Json;

namespace MicrowaveApp.Blazor.Services;

public static class ApiErrorReader
{
    public static async Task<string> ReadMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            return "Autentique-se para executar esta função.";

        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken: cancellationToken);
            return error?.Message ?? "Não foi possível concluir a operação.";
        }
        catch
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return string.IsNullOrWhiteSpace(content) ? "Não foi possível concluir a operação." : content;
        }
    }

    private sealed record ApiError(string ErrorCode, string Message, string? TraceId);
}
