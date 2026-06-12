using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.Enums;

namespace MicrowaveApp.Domain.Entities;

public class Microwave
{
    public HeatingSession? CurrentSession { get; private set; }

    public HeatingSession QuickStart()
    {
        if (CurrentSession?.Status == HeatingStatus.Heating)
            return AddTime();

        if (CurrentSession?.Status == HeatingStatus.Paused)
            return Resume();

        CurrentSession = HeatingSession.QuickStart();
        return CurrentSession;
    }

    public HeatingSession Start(int? timeInSeconds, int? power)
    {
        if (CurrentSession?.Status == HeatingStatus.Heating)
            return AddTime();

        if (CurrentSession?.Status == HeatingStatus.Paused)
            return Resume();

        if (timeInSeconds is null)
            return QuickStart();

        CurrentSession = new HeatingSession(
            timeInSeconds.Value,
            power ?? HeatingSession.DefaultPower);

        return CurrentSession;
    }

    public HeatingSession Start(HeatingProgram program)
    {
        CurrentSession = HeatingSession.FromProgram(program);
        return CurrentSession;
    }

    public HeatingSession AddTime()
    {
        var session = GetRequiredCurrentSession();
        session.AddTime();
        return session;
    }

    public HeatingSession Pause()
    {
        var session = GetRequiredCurrentSession();
        session.Pause();
        return session;
    }

    public HeatingSession? PauseOrCancel()
    {
        if (CurrentSession is null)
            return null;

        if (CurrentSession.Status == HeatingStatus.Heating)
            return Pause();

        if (CurrentSession.Status == HeatingStatus.Paused)
        {
            Cancel();
            return null;
        }

        Cancel();
        return null;
    }

    public HeatingSession Resume()
    {
        var session = GetRequiredCurrentSession();
        session.Resume();
        return session;
    }

    public HeatingSession Advance(int seconds = 1)
    {
        var session = GetRequiredCurrentSession();
        session.Advance(seconds);
        return session;
    }

    public void Cancel()
    {
        CurrentSession?.Cancel();
        CurrentSession = null;
    }

    private HeatingSession GetRequiredCurrentSession()
    {
        return CurrentSession ?? throw new BusinessException("Nenhuma sessão de aquecimento em andamento.", "HEATING_SESSION_NOT_FOUND");
    }
}
