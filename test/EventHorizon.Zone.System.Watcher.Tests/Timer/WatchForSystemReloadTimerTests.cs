namespace EventHorizon.Zone.System.Watcher.Tests.Timer;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Watcher.Check;
using EventHorizon.Zone.System.Watcher.Timer;

using FluentAssertions;

using Xunit;

public class WatchForSystemReloadTimerTests
{
    [Fact]
    public void TestShouldHaveExpectedPeriodWhenCreated()
    {
        // Given
        var expectedPeriod = 5000;
        var expectedTag = "WatchForSystemReload";
        var expectedOnValidationEvent = new IsServerStarted();
        var expectedOnRunEvent = new CheckPendingReloadEvent();

        // When
        var actual = new WatchForSystemReloadTimer();

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
