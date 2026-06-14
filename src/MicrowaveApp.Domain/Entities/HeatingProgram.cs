namespace MicrowaveApp.Domain.Entities;

using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

public class HeatingProgram
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Food { get; private set; } = string.Empty;
    public int TimeInSeconds { get; private set; }
    public int Power { get; private set; }
    public char HeatingChar { get; private set; }
    public string? Instructions { get; private set; }
    public bool IsPresent { get; private set; }
    public bool IsPreset => IsPresent;
    public DateTime CreatedAt { get; private set; }

    private HeatingProgram()
    {
    }
    
    public HeatingProgram(
        string name,
        string food,
        int timeInSeconds,
        int power,
        char heatingChar,
        string? instructions = null,
        bool isPresent = false)
        : this(name, food, HeatingTime.ForProgram(timeInSeconds), new PowerLevel(power), new HeatingCharacter(heatingChar), instructions, isPresent)
    {
    }

    public HeatingProgram(
        string name,
        string food,
        HeatingTime time,
        PowerLevel power,
        HeatingCharacter heatingCharacter,
        string? instructions = null,
        bool isPresent = false)
    {
        Validate(name, food, heatingCharacter);

        Name = name.Trim();
        Food = food.Trim();
        TimeInSeconds = time.Seconds;
        Power = power.Value;
        HeatingChar = heatingCharacter.Value;
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
        IsPresent = isPresent;
        CreatedAt = DateTime.UtcNow;
    }

    public static HeatingProgram CreatePreset(
        int id,
        string name,
        string food,
        int timeInSeconds,
        int power,
        char heatingChar,
        string instructions)
    {
        var program = new HeatingProgram(
            name,
            food,
            HeatingTime.ForProgram(timeInSeconds),
            new PowerLevel(power),
            new HeatingCharacter(heatingChar),
            instructions,
            isPresent: true);

        program.Id = id;
        return program;
    }

    public void Rename(string name)
    {
        EnsureCanChange();

        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Nome do programa é obrigatório.", "PROGRAM_NAME_REQUIRED");

        Name = name.Trim();
    }

    public void Update(
        string name,
        string food,
        int timeInSeconds,
        int power,
        char heatingChar,
        string? instructions = null)
    {
        EnsureCanChange();

        var time = HeatingTime.ForProgram(timeInSeconds);
        var powerLevel = new PowerLevel(power);
        var character = new HeatingCharacter(heatingChar);

        Validate(name, food, character);

        Name = name.Trim();
        Food = food.Trim();
        TimeInSeconds = time.Seconds;
        Power = powerLevel.Value;
        HeatingChar = character.Value;
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
    }

    private void EnsureCanChange()
    {
        if (IsPresent)
            throw new BusinessException("Programas pré-definidos não podem ser alterados.", "PRESET_PROGRAM_CANNOT_CHANGE");
    }

    private static void Validate(string name, string food, HeatingCharacter heatingCharacter)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Nome do programa é obrigatório.", "PROGRAM_NAME_REQUIRED");

        if (string.IsNullOrWhiteSpace(food))
            throw new BusinessException("Alimento é obrigatório.", "PROGRAM_FOOD_REQUIRED");

        if (heatingCharacter.Value == HeatingCharacter.Default)
            throw new BusinessException("Programas de aquecimento não podem usar o caractere padrão.", "PROGRAM_HEATING_CHAR_CANNOT_BE_DEFAULT");
    }
}
