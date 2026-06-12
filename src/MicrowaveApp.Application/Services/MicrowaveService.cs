using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Validators;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Application.Services;

public sealed class MicrowaveService : IMicrowaveService
{
    private readonly Microwave _microwave;
    private readonly IHeatingProgramRepository _programs;

    public MicrowaveService(Microwave microwave, IHeatingProgramRepository programs)
    {
        _microwave = microwave;
        _programs = programs;
    }

    public Task<HeatingSessionResponse> QuickStartAsync(CancellationToken cancellationToken = default)
    {
        var session = _microwave.QuickStart();
        return Task.FromResult(ToResponse(session));
    }

    public Task<HeatingSessionResponse> StartAsync(StartHeatingRequest request, CancellationToken cancellationToken = default)
    {
        HeatingRequestValidator.Validate(request);

        var session = _microwave.Start(request.TimeInSeconds, request.Power);
        return Task.FromResult(ToResponse(session));
    }

    public async Task<HeatingSessionResponse> StartProgramAsync(int programId, CancellationToken cancellationToken = default)
    {
        var program = await _programs.GetByIdAsync(programId, cancellationToken);

        if (program is null)
            throw new BusinessException("Programa de aquecimento não encontrado.", "HEATING_PROGRAM_NOT_FOUND");

        var session = _microwave.Start(program);
        return ToResponse(session);
    }

    public Task<HeatingSessionResponse> AddTimeAsync(CancellationToken cancellationToken = default)
    {
        var session = _microwave.AddTime();
        return Task.FromResult(ToResponse(session));
    }

    public Task<HeatingSessionResponse> PauseAsync(CancellationToken cancellationToken = default)
    {
        var session = _microwave.Pause();
        return Task.FromResult(ToResponse(session));
    }

    public Task<HeatingSessionResponse?> PauseOrCancelAsync(CancellationToken cancellationToken = default)
    {
        var session = _microwave.PauseOrCancel();
        return Task.FromResult(session is null ? null : ToResponse(session));
    }

    public Task<HeatingSessionResponse> ResumeAsync(CancellationToken cancellationToken = default)
    {
        var session = _microwave.Resume();
        return Task.FromResult(ToResponse(session));
    }

    public Task<HeatingSessionResponse> AdvanceAsync(int seconds = 1, CancellationToken cancellationToken = default)
    {
        var session = _microwave.Advance(seconds);
        return Task.FromResult(ToResponse(session));
    }

    public Task CancelAsync(CancellationToken cancellationToken = default)
    {
        _microwave.Cancel();
        return Task.CompletedTask;
    }

    private static HeatingSessionResponse ToResponse(HeatingSession session)
    {
        return new HeatingSessionResponse(
            session.SessionId,
            session.TotalTimeInSeconds,
            session.ReadTimeInSeconds,
            session.RemainingTimeInSeconds,
            FormatTime(session.TotalTimeInSeconds),
            FormatTime(session.RemainingTimeInSeconds),
            session.Power,
            session.HeatingChar,
            session.Status,
            session.HeatingString,
            session.IsPresentProgram,
            session.StartedAt,
            session.PausedAt);
    }

    private static string FormatTime(int totalSeconds)
    {
        if (totalSeconds < 60)
            return totalSeconds.ToString();

        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;

        return $"{minutes}:{seconds:00}";
    }
}
