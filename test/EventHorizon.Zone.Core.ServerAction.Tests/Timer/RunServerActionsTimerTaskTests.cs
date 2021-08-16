namespace EventHorizon.Zone.Core.ServerAction.Tests.Timer
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.ServerAction.Run;
    using EventHorizon.Zone.Core.ServerAction.Timer;

    using FluentAssertions;

    using Xunit;

    public class RunServerActionsTimerTaskTests
    {
        [Fact]
        public void TestCreateOfTimerInstanceMatchesExpectedProperties()
        {
            // Given
            var expectedPeriod = 10;
            var expectedTag = "RunServerActions";
            var expectedOnValidationEvent = new IsServerStarted();
            var expectedOnRunEvent = new RunPendingServerActionsEvent();

            // When
            var runServerActionsTimerTask = new RunServerActionsTimerTask();

            // Then
            runServerActionsTimerTask.Period
                .Should().Be(expectedPeriod);
            runServerActionsTimerTask.Tag
                .Should().Be(expectedTag);
            runServerActionsTimerTask.OnValidationEvent
                .Should().Be(expectedOnValidationEvent);
            runServerActionsTimerTask.OnRunEvent
                .Should().Be(expectedOnRunEvent);
            runServerActionsTimerTask.LogDetails
                .Should().BeFalse();
        }
    }
}
