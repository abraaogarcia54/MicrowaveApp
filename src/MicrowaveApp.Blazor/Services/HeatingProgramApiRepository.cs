using System.Net.Http.Json;
using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Blazor.Services;

public sealed class HeatingProgramApiRepository : IHeatingProgramRepository
{
    private readonly IHeatingProgramService _heatingProgramService;

    public HeatingProgramApiRepository(IHeatingProgramService heatingProgramService)
    {
        _heatingProgramService = heatingProgramService;
    }

    public async Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var responses = await _heatingProgramService.GetAllAsync(cancellationToken);
        return responses.Select(ToEntity).ToArray();
    }

    public async Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var programs = await _heatingProgramService.GetAllAsync(cancellationToken);
        var program = programs.FirstOrDefault(program => program.Id == id);

        return program is null ? null : ToEntity(program);
    }

    public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Cadastro deve ser feito via IHeatingProgramService.");
    }

    public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Alteração de programas não é suportada nesta etapa.");
    }

    private static HeatingProgram ToEntity(HeatingProgramResponse response)
    {
        return new HeatingProgram(
            response.Name,
            response.Food,
            response.TimeInSeconds,
            response.Power,
            response.HeatingChar,
            response.Instructions,
            response.IsPresent);
    }
}
