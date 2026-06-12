namespace MicrowaveApp.Application.DTOs;

public sealed record StartHeatingRequest(int? TimeInSeconds = null, int? Power = null);
