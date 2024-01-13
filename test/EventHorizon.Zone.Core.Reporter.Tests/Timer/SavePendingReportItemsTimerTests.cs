namespace EventHorizon.Zone.Core.Reporter.Tests.Timer;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Reporter.Save;
using EventHorizon.Zone.Core.Reporter.Timer;

using FluentAssertions;

using Xunit;

public class SavePendingReportItemsTimerTests
{
    [Fact]
    public void TestShouldHaveExpectedPropertiesWhenCreatedNew()
    {
        // Given
        var expectedPeriod = 15000;
        var expectedTag = "SavePendingReportItems";
        var expectedOnValidationEvent = new IsServerStarted();
        var expectedOnRunEvent = new SavePendingReportItemsEvent();

        // When
        var actual = new SavePendingReportItemsTimer();

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
            .Should().BeTrue();
    }
}
