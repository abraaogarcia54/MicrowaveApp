using MicrowaveApp.Domain.Exceptions;
using MicrowaveApp.Domain.ValueObjects;

namespace MicrowaveApp.Domain.Tests;

public class HeatingCharacterTests
{
    [Fact]
    public void Constructor_ShouldRejectWhitespace()
    {
        var act = () => new HeatingCharacter(' ');

        act.Should().Throw<BusinessException>();
    }
}
