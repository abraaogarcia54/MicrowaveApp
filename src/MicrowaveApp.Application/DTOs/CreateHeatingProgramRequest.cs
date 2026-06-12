namespace MicrowaveApp.Application.DTOs;

public sealed record CreateHeatingProgramRequest(
    string Name,
    string Food,
    int TimeInSeconds,
    int Power,
    char HeatingChar,
    string? Instructions);
