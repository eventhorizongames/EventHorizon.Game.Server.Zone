namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Timer
{
    using Xunit;
    using EventHorizon.Zone.System.Agent.Move.Timer;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using FluentAssertions;

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
            actual.Period
                .Should().Be(expectedPeriod);
            actual.Tag
                .Should().Be(expectedTag);
            actual.OnValidationEvent
                .Should().Be(expectedValidationEvent);
            actual.OnRunEvent
                .Should().Be(expectedEvent);
            actual.LogDetails
                .Should().BeFalse();
        }
    }
}