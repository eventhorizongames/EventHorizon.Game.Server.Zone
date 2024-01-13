namespace EventHorizon.Zone.System.Agent.Tests.Monitor.Timer;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Agent.Monitor.Path;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;

using FluentAssertions;

using Xunit;

public class CheckForStaleAgentPathTimerTests
{
    [Fact]
    public void ShouldHaveExpectedPropertiesWhenInstantiated()
    {
        // Given 
        var expectedPeriod = 1000;
        var expectedTag = "CheckForStaleAgentPath";
        var expectedOnValidationEvent = new IsServerStarted();
        var expectedOnRunEvent = new CheckForStaleAgentPath();

        // When
        var actual = new CheckForStaleAgentPathTimer();

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
