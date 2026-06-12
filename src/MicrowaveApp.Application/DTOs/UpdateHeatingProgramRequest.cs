namespace MicrowaveApp.Application.DTOs;

public sealed record UpdateHeatingProgramRequest(
    string Name,
    string Food,
    int TimeInSeconds,
    int Power,
    char HeatingChar,
    string? Instructions);
