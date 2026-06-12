namespace MicrowaveApp.Application.DTOs;

public sealed record HeatingProgramResponse(
    int Id,
    string Name,
    string Food,
    int TimeInSeconds,
    int Power,
    char HeatingChar,
    string? Instructions,
    bool IsPresent,
    DateTime CreatedAt);
