using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.ValueObjects;

public readonly record struct HeatingCharacter
{
    public const char Default = '.';

    public char Value { get; }

    public HeatingCharacter(char value)
    {
        if (char.IsWhiteSpace(value))
            throw new BusinessException("Caractere de aquecimento é obrigatório.", "INVALID_HEATING_CHAR");

        Value = value;
    }

    public static implicit operator char(HeatingCharacter heatingCharacter) => heatingCharacter.Value;
}
