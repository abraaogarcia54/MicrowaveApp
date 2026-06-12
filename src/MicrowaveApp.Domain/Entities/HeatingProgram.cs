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
    public DateTime CreatedAt { get; private set; }

    
    public HeatingProgram(
        string name,
        string food,
        int timeInSeconds,
        int power,
        char heatingChar,
        string? instructions = null,
        bool isPresent = false)
        : this(name, food, new HeatingTime(timeInSeconds), new PowerLevel(power), new HeatingCharacter(heatingChar), instructions, isPresent)
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
        Validate(name, food);

        Name = name.Trim();
        Food = food.Trim();
        TimeInSeconds = time.Seconds;
        Power = power.Value;
        HeatingChar = heatingCharacter.Value;
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
        IsPresent = isPresent;
        CreatedAt = DateTime.UtcNow;
    }

    public void Rename(string name)
    {
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
        var time = new HeatingTime(timeInSeconds);
        var powerLevel = new PowerLevel(power);
        var character = new HeatingCharacter(heatingChar);

        Validate(name, food);

        Name = name.Trim();
        Food = food.Trim();
        TimeInSeconds = time.Seconds;
        Power = powerLevel.Value;
        HeatingChar = character.Value;
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
    }

    private static void Validate(string name, string food)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Nome do programa é obrigatório.", "PROGRAM_NAME_REQUIRED");

        if (string.IsNullOrWhiteSpace(food))
            throw new BusinessException("Alimento é obrigatório.", "PROGRAM_FOOD_REQUIRED");
    }
}
