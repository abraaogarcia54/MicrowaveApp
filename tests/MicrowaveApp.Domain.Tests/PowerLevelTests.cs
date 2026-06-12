using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Domain.Tests;

public class PowerLevelTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Constructor_ShouldRejectInvalidPower(int value)
    {
        var act = () => new PowerLevel(value);

        act.Should().Throw<BusinessException>();
    }
}
