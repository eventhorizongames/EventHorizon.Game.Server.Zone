namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Save;
    using EventHorizon.Zone.System.Agent.Save.Events;
    using FluentAssertions;
    using Xunit;

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
            saveAgentStateTimerTask.Period
                .Should().Be(expectedPeriod);
            saveAgentStateTimerTask.Tag
                .Should().Be(expectedTag);
            saveAgentStateTimerTask.OnValidationEvent
                .Should().Be(expectedOnValidationEvent);
            saveAgentStateTimerTask.OnRunEvent
                .Should().Be(expectedOnRunEvent);
            saveAgentStateTimerTask.LogDetails
                .Should().BeTrue();
        }
    }
}