using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.ValueObjects;

public readonly record struct HeatingTime
{
    public const int MinSeconds = 1;
    public const int MaxSeconds = 120;

    public int Seconds { get; }

    public HeatingTime(int seconds)
    {
        if (seconds is < MinSeconds or > MaxSeconds)
            throw new BusinessException("Tempo deve estar entre 1 e 120 segundos.", "INVALID_HEATING_TIME");

        Seconds = seconds;
    }

    public static implicit operator int(HeatingTime heatingTime) => heatingTime.Seconds;
}
