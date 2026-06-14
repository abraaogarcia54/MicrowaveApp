using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Validators;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Application.Services;

public sealed class HeatingProgramService : IHeatingProgramService
{
    private readonly IHeatingProgramRepository _programs;

    public HeatingProgramService(IHeatingProgramRepository programs)
    {
        _programs = programs;
    }

    public async Task<IReadOnlyCollection<HeatingProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var programs = await _programs.GetAllAsync(cancellationToken);
        return programs.Select(ToResponse).ToArray();
    }

    public async Task<HeatingProgramResponse> CreateAsync(CreateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        HeatingProgramValidator.Validate(request);
        await EnsureHeatingCharacterIsUniqueAsync(request.HeatingChar, cancellationToken: cancellationToken);

        var program = new HeatingProgram(
            request.Name,
            request.Food,
            request.TimeInSeconds,
            request.Power,
            request.HeatingChar,
            request.Instructions);

        await _programs.AddAsync(program, cancellationToken);

        return ToResponse(program);
    }

    public async Task<HeatingProgramResponse> UpdateAsync(int id, UpdateHeatingProgramRequest request, CancellationToken cancellationToken = default)
    {
        HeatingProgramValidator.Validate(request);

        var program = await _programs.GetByIdAsync(id, cancellationToken);

        if (program is null)
            throw new BusinessException("Programa de aquecimento não encontrado.", "HEATING_PROGRAM_NOT_FOUND");

        await EnsureHeatingCharacterIsUniqueAsync(request.HeatingChar, id, cancellationToken);

        program.Update(
            request.Name,
            request.Food,
            request.TimeInSeconds,
            request.Power,
            request.HeatingChar,
            request.Instructions);

        await _programs.UpdateAsync(program, cancellationToken);

        return ToResponse(program);
    }

    private async Task EnsureHeatingCharacterIsUniqueAsync(
        char heatingChar,
        int? currentProgramId = null,
        CancellationToken cancellationToken = default)
    {
        var programs = await _programs.GetAllAsync(cancellationToken);
        var duplicated = programs.Any(program =>
            program.HeatingChar == heatingChar &&
            (!currentProgramId.HasValue || program.Id != currentProgramId.Value));

        if (duplicated)
            throw new BusinessException("Caractere de aquecimento já está em uso.", "HEATING_CHAR_ALREADY_IN_USE");
    }

    private static HeatingProgramResponse ToResponse(HeatingProgram program)
    {
        return new HeatingProgramResponse(
            program.Id,
            program.Name,
            program.Food,
            program.TimeInSeconds,
            program.Power,
            program.HeatingChar,
            program.Instructions,
            program.IsPresent,
            program.CreatedAt);
    }
}
