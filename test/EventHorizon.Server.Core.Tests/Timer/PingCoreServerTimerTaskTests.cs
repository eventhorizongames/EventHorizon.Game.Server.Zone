namespace EventHorizon.Server.Core.Tests.Timer;

using EventHorizon.Server.Core.Events.Ping;
using EventHorizon.Server.Core.Timer;
using EventHorizon.Zone.Core.Events.Lifetime;

using FluentAssertions;

using Xunit;

public class PingCoreServerTimerTaskTests
{
    [Fact]
    public void ShouldHaveExpectedValuesWhenCreated()
    {
        // Given
        var expectedPeriod = 5000;
        var expectedTag = "PingCoreServer";
        var expectedOnValidationEvent = new IsServerStarted();
        var expectedOnRunEvent = new PingCoreServer();

        // When
        var actual = new PingCoreServerTimerTask();

        // Then
        actual.Period
            .Should().Be(expectedPeriod);
        actual.Tag
            .Should().Be(expectedTag);
        actual.OnValidationEvent
            .Should().Be(expectedOnValidationEvent);
        actual.OnRunEvent
            .Should().Be(expectedOnRunEvent);
        actual.LogDetails
            .Should().BeFalse();
    }
}
