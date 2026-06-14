using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.Tests;

public class HeatingSessionTests
{
    [Fact]
    public void AddTime_ShouldRejectPresetProgramSession()
    {
        var program = HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar estouros.");
        var session = HeatingSession.FromProgram(program);

        var act = () => session.AddTime();

        act.Should().Throw<BusinessException>();
    }
}
