using Xunit;
using EventHorizon.Zone.System.Agent.Move.Impl;
using EventHorizon.Zone.System.Agent.Events.Move;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimeTests
    {
        [Fact]
        public void TestStart_ShouldPublishMoveRegisteredAgentsEventAfterSetAmountOfTime()
        {
            // Given
            var expectedPeriod = 100;
            var expectedEvent = new MoveRegisteredAgentsEvent();

            // When
            var actual = new MoveRegisteredAgentsTimer();

            // Then
            Assert.Equal(expectedPeriod, actual.Period);
            Assert.Equal(expectedEvent, actual.OnRunEvent);
        }

    }
}