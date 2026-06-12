using MicrowaveApp.Domain.Enums;
using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Domain.Entities;

public class HeatingSession
{
    public const int QuickStartTimeInSeconds = 30;
    public const int DefaultPower = 10;
    public const int MaxManualTimeInSeconds = 120;
    public const int MinManualTimeInSeconds = 1;
    public const int AddTimeOnClickSeconds = 30;
    public const string CompletionMessage = "Aquecimento concluído";

    public string SessionId { get; private set; } = string.Empty;
    public int TotalTimeInSeconds { get; private set; }
    public int ReadTimeInSeconds { get; private set; }
    public int Power { get; private set; }
    public char HeatingChar { get; private set; }
    public HeatingStatus Status { get; private set; }
    public string HeatingString { get; private set; } = string.Empty;
    public bool IsPresentProgram { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? PausedAt { get; private set; }
    
    public HeatingSession(int totalTimeInSeconds, int power, char heatingChar = HeatingCharacter.Default, bool isPresentProgram = false)
        : this(new HeatingTime(totalTimeInSeconds), new PowerLevel(power), new HeatingCharacter(heatingChar), isPresentProgram)
    {
    }

    public HeatingSession(
        HeatingTime totalTime,
        PowerLevel power,
        HeatingCharacter heatingCharacter,
        bool isPresentProgram = false)
    {
        SessionId = Guid.NewGuid().ToString("N");
        TotalTimeInSeconds = totalTime.Seconds;
        ReadTimeInSeconds = 0;
        Power = power.Value;
        HeatingChar = heatingCharacter.Value;
        Status = HeatingStatus.Heating;
        HeatingString = string.Empty;
        IsPresentProgram = isPresentProgram;
        StartedAt = DateTime.UtcNow;
    }

    public static HeatingSession QuickStart()
    {
        return new HeatingSession(QuickStartTimeInSeconds, DefaultPower);
    }

    public static HeatingSession FromProgram(HeatingProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);

        return new HeatingSession(
            program.TimeInSeconds,
            program.Power,
            program.HeatingChar,
            program.IsPresent);
    }

    public int RemainingTimeInSeconds => TotalTimeInSeconds - ReadTimeInSeconds;
    public bool IsFinished => Status is HeatingStatus.Completed or HeatingStatus.Cancelled;

    public void AddTime()
    {
        EnsureCanChangeTime();

        if (IsPresentProgram)
            throw new BusinessException("Programas pré-definidos não permitem acréscimo de tempo.", "PRESET_PROGRAM_CANNOT_ADD_TIME");

        TotalTimeInSeconds += AddTimeOnClickSeconds;
    }

    public void Pause()
    {
        if (Status != HeatingStatus.Heating)
            throw new BusinessException("Apenas uma sessão em aquecimento pode ser pausada.", "SESSION_CANNOT_PAUSE");

        Status = HeatingStatus.Paused;
        PausedAt = DateTime.UtcNow;
    }

    public void Resume()
    {
        if (Status != HeatingStatus.Paused)
            throw new BusinessException("Apenas uma sessão pausada pode ser retomada.", "SESSION_CANNOT_RESUME");

        Status = HeatingStatus.Heating;
        PausedAt = null;
    }

    public void Cancel()
    {
        if (IsFinished)
            return;

        Status = HeatingStatus.Cancelled;
        PausedAt = null;
    }

    public void Advance(int seconds = 1)
    {
        if (seconds <= 0)
            throw new BusinessException("Tempo de avanço deve ser maior que zero.", "INVALID_ADVANCE_TIME");

        if (Status != HeatingStatus.Heating)
            throw new BusinessException("Apenas uma sessão em aquecimento pode avançar.", "SESSION_CANNOT_ADVANCE");

        var secondsToAdvance = Math.Min(seconds, RemainingTimeInSeconds);

        for (var i = 0; i < secondsToAdvance; i++)
        {
            AppendHeatingPulse();
        }

        ReadTimeInSeconds += secondsToAdvance;

        if (ReadTimeInSeconds >= TotalTimeInSeconds)
        {
            Status = HeatingStatus.Completed;
            HeatingString = string.IsNullOrWhiteSpace(HeatingString)
                ? CompletionMessage
                : $"{HeatingString} {CompletionMessage}";
        }
    }

    private void AppendHeatingPulse()
    {
        var pulse = new string(HeatingChar, Power);
        HeatingString = string.IsNullOrEmpty(HeatingString) ? pulse : $"{HeatingString} {pulse}";
    }

    private void EnsureCanChangeTime()
    {
        if (Status != HeatingStatus.Heating)
            throw new BusinessException("Tempo só pode ser alterado durante o aquecimento.", "SESSION_TIME_CANNOT_CHANGE");
    }

}
