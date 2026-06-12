using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.ValueObjects;

public readonly record struct PowerLevel
{
    public const int Min = 1;
    public const int Max = 10;

    public int Value { get; }

    public PowerLevel(int value)
    {
        if (value is < Min or > Max)
            throw new BusinessException("Potência deve estar entre 1 e 10.", "INVALID_HEATING_POWER");

        Value = value;
    }

    public static implicit operator int(PowerLevel powerLevel) => powerLevel.Value;
}
