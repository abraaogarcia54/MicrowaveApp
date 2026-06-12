using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Application.Validators;

public static class HeatingRequestValidator
{
    public static void Validate(StartHeatingRequest request)
    {
        if (request.TimeInSeconds is not null)
            _ = new HeatingTime(request.TimeInSeconds.Value);

        if (request.Power is not null)
            _ = new PowerLevel(request.Power.Value);
    }
}
