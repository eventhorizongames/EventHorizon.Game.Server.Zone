namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.ClientAction
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;

    using FluentAssertions;

    using Xunit;

    public class ClientSkillActionEventTests
    {
        [Fact]
        public void ShouldHaveExpectedWhenCreated()
        {
            // Given
            var action = "action";
            var data = new { };

            var expectedAction = action;
            var expectedData = data;

            // When
            var actual = new ClientSkillActionEvent
            {
                Action = action,
                Data = data,
            };

            // Then
            actual.Action
                .Should().Be(expectedAction);
            actual.Data
                .Should().Be(expectedData);
        }
    }
}
