namespace EventHorizon.Zone.System.Agent.Tests.Monitor.Timer
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Monitor.Path;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
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