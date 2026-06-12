namespace MicrowaveApp.Application.DTOs;

public sealed record LoginResponse(int UserId, string Username, string? Token = null);
