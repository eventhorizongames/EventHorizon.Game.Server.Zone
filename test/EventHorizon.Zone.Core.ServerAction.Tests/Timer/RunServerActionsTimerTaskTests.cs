using EventHorizon.Zone.Core.ServerAction.Run;
using EventHorizon.Zone.Core.ServerAction.Timer;
using Xunit;

namespace EventHorizon.Zone.Core.ServerAction.Tests.Timer
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
            Assert.Equal(
                expectedPeriod,
                runServerActionsTimerTask.Period
            );
            Assert.Equal(
                expectedOnRunEvent,
                runServerActionsTimerTask.OnRunEvent
            );
        }
    }
}