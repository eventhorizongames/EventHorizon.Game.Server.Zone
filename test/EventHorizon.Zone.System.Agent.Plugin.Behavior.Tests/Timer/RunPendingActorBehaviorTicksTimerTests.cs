namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Timer
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
    using FluentAssertions;
    using Xunit;

    public class RunPendingActorBehaviorTicksTimerTests
    {
        [Fact]
        public void ShouldHaveExpectedPropertiesWhenInstantiated()
        {
            // Given 
            var expectedPeriod = 100;
            var expectedTag = "RunPendingActorBehaviorTicks";
            var expectedOnValidationEvent = new IsServerStarted();
            var expectedOnRunEvent = new RunPendingActorBehaviorTicks();

            // When
            var actual = new RunPendingActorBehaviorTicksTimer();

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
}