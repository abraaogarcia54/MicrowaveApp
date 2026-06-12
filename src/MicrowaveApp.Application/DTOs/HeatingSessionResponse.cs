using MicrowaveApp.Domain.Enums;

namespace MicrowaveApp.Application.DTOs;

public sealed record HeatingSessionResponse(
    string SessionId,
    int TotalTimeInSeconds,
    int ReadTimeInSeconds,
    int RemainingTimeInSeconds,
    string TotalTimeDisplay,
    string RemainingTimeDisplay,
    int Power,
    char HeatingChar,
    HeatingStatus Status,
    string HeatingString,
    bool IsPresentProgram,
    DateTime StartedAt,
    DateTime? PausedAt);
