using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Blazor.Services;

public sealed class InMemoryHeatingProgramRepository : IHeatingProgramRepository
{
    private readonly List<HeatingProgram> _programs =
    [
        HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento."),
        HeatingProgram.CreatePreset(
            2,
            "Leite",
            "Leite",
            300,
            5,
            '~',
            "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras."),
        HeatingProgram.CreatePreset(
            3,
            "Carnes de boi",
            "Carne em pedaço ou fatias",
            840,
            4,
            '#',
            "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."),
        HeatingProgram.CreatePreset(
            4,
            "Frango",
            "Frango (qualquer corte)",
            480,
            7,
            '@',
            "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."),
        HeatingProgram.CreatePreset(
            5,
            "Feijão",
            "Feijão congelado",
            480,
            9,
            '+',
            "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas.")
    ];

    public Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<HeatingProgram>>(_programs);
    }

    public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var program = _programs.FirstOrDefault(program => program.Id == id);
        return Task.FromResult(program);
    }

    public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        _programs.Add(program);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
