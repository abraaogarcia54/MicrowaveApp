namespace MicrowaveApp.Application.Interfaces;

public interface IExceptionLogger
{
    Task LogAsync(
        Exception exception,
        string traceId,
        string path,
        string method,
        CancellationToken cancellationToken = default);
}
