namespace MicrowaveApp.Infrastructure.Logging;

public sealed class FileExceptionLogger
{
    public Task LogAsync(Exception exception, CancellationToken cancellationToken = default)
    {
        Console.Error.WriteLine(exception);
        return Task.CompletedTask;
    }
}
