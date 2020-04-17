namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Timer
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
    using Xunit;

    public class RunPendingActorBehaviorTicksTimerTests
    {
        [Fact]
        public void ShouldHaveExpectedPropertiesWhenInstantiated()
        {
            // Given 
            var expectedPeriod = 100;
            var expectedTag = "RunUpdateOnAllBehaviorTrees";
            var expectedOnValidationEvent = new IsServerStarted();
            var expectedOnRunEvent = new RunPendingActorBehaviorTicks();

            // When
            var actual = new RunPendingActorBehaviorTicksTimer();

            // Then
            Assert.Equal(
                expectedPeriod,
                actual.Period
            );
            Assert.Equal(
                expectedTag,
                actual.Tag
            );
            Assert.Equal(
                expectedOnValidationEvent,
                actual.OnValidationEvent
            );
            Assert.Equal(
                expectedOnRunEvent,
                actual.OnRunEvent
            );
        }
    }
}