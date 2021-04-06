namespace EventHorizon.Game.Tests.Timer
{
    using EventHorizon.Game.Capture;
    using EventHorizon.Game.Timer;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using FluentAssertions;
    using Xunit;

    public class RunPlayerListCaptureLogicTimerTaskTests
    {
        [Fact]
        public void ShouldHaveExpectedPropertiesWhenCreated()
        {
            // Given
            var expectedPeriod = 1000;
            var expectedTag = "RunPlayerListCaptureLogicTimerTask";
            var expectedOnValidateEvent = new IsServerStarted();
            var expectedOnRunEvent = new RunCaptureLogicForAllPlayers();

            // When
            var actual = new RunPlayerListCaptureLogicTimerTask();

            // Then
            actual.Period
                .Should().Be(expectedPeriod);
            actual.Tag
                .Should().Be(expectedTag);
            actual.OnValidationEvent
                .Should().Be(expectedOnValidateEvent);
            actual.OnRunEvent
                .Should().Be(expectedOnRunEvent);
            actual.LogDetails
                .Should().BeFalse();
        }
    }
}
