using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Application.Validators;

public static class HeatingProgramValidator
{
    public static void Validate(CreateHeatingProgramRequest request)
    {
        Validate(request.Name, request.Food, request.TimeInSeconds, request.Power, request.HeatingChar);
    }

    public static void Validate(UpdateHeatingProgramRequest request)
    {
        Validate(request.Name, request.Food, request.TimeInSeconds, request.Power, request.HeatingChar);
    }

    private static void Validate(string name, string food, int timeInSeconds, int power, char heatingChar)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Nome do programa é obrigatório.", "PROGRAM_NAME_REQUIRED");

        if (string.IsNullOrWhiteSpace(food))
            throw new BusinessException("Alimento é obrigatório.", "PROGRAM_FOOD_REQUIRED");

        _ = new HeatingTime(timeInSeconds);
        _ = new PowerLevel(power);
        _ = new HeatingCharacter(heatingChar);
    }
}
