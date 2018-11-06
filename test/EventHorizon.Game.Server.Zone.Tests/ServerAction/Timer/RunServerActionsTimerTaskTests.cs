using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.ServerAction.Run;
using EventHorizon.Game.Server.Zone.ServerAction.Timer;

namespace EventHorizon.Game.Server.Zone.Tests.ServerAction.Timer
{
    public class RunServerActionsTimerTaskTests
    {
        [Fact]
        public void TestCreateOfTimerInstanceMatchesExpectedProperties()
        {
            // Given
            var expectedPeriod = 10;
            var expectedOnRunEvent = new RunPendingServerActionsEvent();

            // When
            var runServerActionsTimerTask = new RunServerActionsTimerTask();

            // Then
            Assert.Equal(expectedPeriod, runServerActionsTimerTask.Period);
            Assert.Equal(expectedOnRunEvent, runServerActionsTimerTask.OnRunEvent);
        }
    }
}