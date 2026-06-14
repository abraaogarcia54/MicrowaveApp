using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Domain.Tests;

public class HeatingProgramTests
{
    [Fact]
    public void CreatePreset_ShouldAllowTimeGreaterThanManualLimit()
    {
        var program = HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar estouros.");

        program.TimeInSeconds.Should().Be(180);
        program.IsPresent.Should().BeTrue();
    }

    [Fact]
    public void CreatePreset_ShouldRejectDefaultHeatingCharacter()
    {
        var act = () => HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '.',
            "Observar estouros.");

        act.Should().Throw<BusinessException>();
    }

    [Fact]
    public void Update_ShouldRejectPresetProgramChanges()
    {
        var program = HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar estouros.");

        var act = () => program.Update("Outro", "Outro", 60, 5, '#');

        act.Should().Throw<BusinessException>();
    }
}
