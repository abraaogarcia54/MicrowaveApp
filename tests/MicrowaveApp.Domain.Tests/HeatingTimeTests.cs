using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Domain.Tests;

public class HeatingTimeTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(121)]
    public void Constructor_ShouldRejectInvalidTime(int seconds)
    {
        var act = () => new HeatingTime(seconds);

        act.Should().Throw<BusinessException>();
    }
}
