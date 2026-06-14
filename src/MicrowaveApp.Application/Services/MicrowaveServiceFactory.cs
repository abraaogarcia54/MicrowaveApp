using MicrowaveApp.Application.Interfaces;

namespace MicrowaveApp.Application.Services;

public sealed class MicrowaveServiceFactory : IMicrowaveServiceFactory
{
    private readonly IMicrowaveSessionStore _sessionStore;
    private readonly IHeatingProgramRepository _programs;

    public MicrowaveServiceFactory(IMicrowaveSessionStore sessionStore, IHeatingProgramRepository programs)
    {
        _sessionStore = sessionStore;
        _programs = programs;
    }

    public async Task<IMicrowaveService> CreateForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var microwave = await _sessionStore.GetOrCreateAsync(userId, cancellationToken);
        return new MicrowaveService(microwave, _programs);
    }
}
