using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.ValueObjects;

public readonly record struct HeatingTime
{
    public const int MinSeconds = 1;
    public const int MaxManualSeconds = 120;

    public int Seconds { get; }

    public HeatingTime(int seconds)
        : this(seconds, enforceManualLimit: true)
    {
    }

    private HeatingTime(int seconds, bool enforceManualLimit)
    {
        if (seconds < MinSeconds)
            throw new BusinessException("Tempo deve ser maior que zero.", "INVALID_HEATING_TIME");

        if (enforceManualLimit && seconds > MaxManualSeconds)
            throw new BusinessException("Tempo deve estar entre 1 e 120 segundos.", "INVALID_HEATING_TIME");

        Seconds = seconds;
    }

    public static HeatingTime ForManualInput(int seconds)
    {
        return new HeatingTime(seconds, enforceManualLimit: true);
    }

    public static HeatingTime ForProgram(int seconds)
    {
        return new HeatingTime(seconds, enforceManualLimit: false);
    }

    public static implicit operator int(HeatingTime heatingTime) => heatingTime.Seconds;
}
