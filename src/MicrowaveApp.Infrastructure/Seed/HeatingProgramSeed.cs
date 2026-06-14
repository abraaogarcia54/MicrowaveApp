using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MicrowaveApp.Infrastructure.Seed;

public static class HeatingProgramSeed
{
    public static IReadOnlyCollection<HeatingProgram> CreateDefaultPrograms()
    {
        return ProgramDefinitions
            .Select((definition, index) => HeatingProgram.CreatePreset(
                index + 1,
                definition.Name,
                definition.Food,
                definition.TimeInSeconds,
                definition.Power,
                definition.HeatingChar,
                definition.Instructions))
            .ToArray();
    }

    public static async Task ApplyAsync(MicrowaveDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var existingPresetNames = await dbContext.HeatingPrograms
            .Where(program => program.IsPresent)
            .Select(program => program.Name)
            .ToArrayAsync(cancellationToken);
        var existingNames = existingPresetNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missingPrograms = ProgramDefinitions
            .Where(program => !existingNames.Contains(program.Name))
            .Select(program => new HeatingProgram(
                program.Name,
                program.Food,
                program.TimeInSeconds,
                program.Power,
                program.HeatingChar,
                program.Instructions,
                isPresent: true))
            .ToArray();

        if (missingPrograms.Length == 0)
            return;

        await dbContext.HeatingPrograms.AddRangeAsync(missingPrograms, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static readonly HeatingProgramDefinition[] ProgramDefinitions =
    [
        new(
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento."),
        new(
            "Leite",
            "Leite",
            300,
            5,
            '~',
            "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras."),
        new(
            "Carnes de boi",
            "Carne em pedaço ou fatias",
            840,
            4,
            '#',
            "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."),
        new(
            "Frango",
            "Frango (qualquer corte)",
            480,
            7,
            '@',
            "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."),
        new(
            "Feijão",
            "Feijão congelado",
            480,
            9,
            '+',
            "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas.")
    ];

    private sealed record HeatingProgramDefinition(
        string Name,
        string Food,
        int TimeInSeconds,
        int Power,
        char HeatingChar,
        string Instructions);
}
