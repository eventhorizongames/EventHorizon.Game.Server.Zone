using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Agent.Save;
using EventHorizon.Zone.System.Agent.Save.Events;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save
{
    public class SaveAgentStateTimerTaskTests
    {
        [Fact]
        public void TestShouldBeExpectedTimerString()
        {
            // Given
            var expectedPeriod = 5000;
            var expectedTag = "SaveAgentStateEvent";
            var expectedOnValidationEvent = new IsServerStarted();
            var expectedOnRunEvent = new SaveAgentStateEvent();

            // When
            var saveAgentStateTimerTask = new SaveAgentStateTimerTask();

            // Then
            Assert.Equal(
                expectedPeriod, saveAgentStateTimerTask.Period
            );
            Assert.Equal(
                expectedTag, saveAgentStateTimerTask.Tag
            );
            Assert.Equal(
                expectedOnValidationEvent, saveAgentStateTimerTask.OnValidationEvent
            );
            Assert.Equal(
                expectedOnRunEvent, saveAgentStateTimerTask.OnRunEvent
            );
        }
    }
}