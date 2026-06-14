using System.Text;
using Microsoft.Extensions.Configuration;
using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Infrastructure.Logging;

public sealed class FileExceptionLogger : IExceptionLogger
{
    private readonly string _logDirectory;

    public FileExceptionLogger(IConfiguration configuration)
    {
        _logDirectory = configuration["ExceptionLogging:Directory"]
            ?? Path.Combine(AppContext.BaseDirectory, "logs");
    }

    public async Task LogAsync(
        Exception exception,
        string traceId,
        string path,
        string method,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_logDirectory);

        var filePath = Path.Combine(_logDirectory, $"exceptions-{DateTime.UtcNow:yyyyMMdd}.log");
        var content = new StringBuilder()
            .AppendLine("========================================")
            .AppendLine($"TraceId: {traceId}")
            .AppendLine($"TimestampUtc: {DateTime.UtcNow:O}")
            .AppendLine($"Method: {method}")
            .AppendLine($"Path: {path}")
            .AppendLine($"Exception: {exception.GetType().FullName}")
            .AppendLine($"Message: {exception.Message}")
            .AppendLine($"InnerException: {exception.InnerException}")
            .AppendLine("StackTrace:")
            .AppendLine(exception.StackTrace)
            .AppendLine()
            .ToString();

        await File.AppendAllTextAsync(filePath, content, cancellationToken);
    }
}
