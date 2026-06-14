namespace MicrowaveApp.Application.Interfaces;

public interface IMicrowaveServiceFactory
{
    Task<IMicrowaveService> CreateForUserAsync(string userId, CancellationToken cancellationToken = default);
}
