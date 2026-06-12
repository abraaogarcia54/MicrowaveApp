using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.Seed;

public static class HeatingProgramSeed
{
    public static IReadOnlyCollection<HeatingProgram> CreateDefaultPrograms()
    {
        return
        [
            new HeatingProgram("Pipoca", "Pipoca de micro-ondas", 120, 7, '*', "Observar o intervalo entre os estouros.", isPresent: true),
            new HeatingProgram("Leite", "Leite", 60, 5, '~', "Usar recipiente adequado.", isPresent: true)
        ];
    }
}
