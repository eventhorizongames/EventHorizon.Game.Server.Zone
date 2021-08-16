namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Query
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class FindSkillByIdHandlerTests
    {
        [Fact]
        public async Task ShouldReturnSkillByIdFromSkillRespositoryWhenRequestIsHandled()
        {
            // Given
            var skillId = "skill-id";
            var expected = new SkillInstance
            {
                Id = skillId,
            };

            var skillRepositoryMock = new Mock<SkillRepository>();

            skillRepositoryMock.Setup(
                mock => mock.Find(
                    skillId
                )
            ).Returns(
                expected
            );

            // When
            var handler = new FindSkillByIdHandler(
                skillRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindSkillByIdEvent
                {
                    SkillId = skillId
                },
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
