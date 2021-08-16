namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Save
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Save;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class SaveCombatSkillHandlerTests
    {
        [Fact]
        public async Task ShouldSetSkillFromRequestWhenRequestIsHandled()
        {
            // Given
            var skill = new SkillInstance();
            var expected = skill;

            var skillRepositoryMock = new Mock<SkillRepository>();

            // When
            var handler = new SaveCombatSkillHandler(
                skillRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new SaveCombatSkillEvent(
                    skill
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
            skillRepositoryMock.Verify(
                mock => mock.Set(
                    expected
                )
            );
        }
    }
}
