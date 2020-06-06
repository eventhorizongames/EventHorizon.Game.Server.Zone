namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.ClientAction
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
    using FluentAssertions;
    using Xunit;

    public class ClientActionRunSkillActionForConnectionEventTests
    {
        [Fact]
        public void ShouldHaveExpectedWhenCreated()
        {
            // Given
            var connectionId = "connection-id";
            var action = "action";
            var data = new { };

            var expectedConnectionId = connectionId;
            var expectedAction = action;
            var expectedData = data;

            // When
            var actual = new ClientActionRunSkillActionForConnectionEvent(
                connectionId,
                action,
                data
            );

            // Then
            actual.ConnectionId
                .Should().Be(expectedConnectionId);
            actual.Action
                .Should().Be(expectedAction);
            actual.Data
                .Should().Be(expectedData);
        }
    }
}