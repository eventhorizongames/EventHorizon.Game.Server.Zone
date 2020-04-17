namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Timer
{
    using Xunit;
    using EventHorizon.Zone.System.Agent.Move.Timer;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
    using EventHorizon.Zone.Core.Events.Lifetime;

    public class MoveRegisteredAgentsTimeTests
    {
        [Fact]
        public void TestStart_ShouldPublishMoveRegisteredAgentsEventAfterSetAmountOfTime()
        {
            // Given
            var expectedPeriod = 50;
            var expectedTag = "MoveRegisteredAgents";
            var expectedValidationEvent = new IsServerStarted();
            var expectedEvent = new MoveRegisteredAgentsEvent();

            // When
            var actual = new MoveRegisteredAgentsTimer();

            // Then
            Assert.Equal(expectedPeriod, actual.Period);
            Assert.Equal(expectedTag, actual.Tag);
            Assert.Equal(expectedValidationEvent, actual.OnValidationEvent);
            Assert.Equal(expectedEvent, actual.OnRunEvent);
        }
    }
}