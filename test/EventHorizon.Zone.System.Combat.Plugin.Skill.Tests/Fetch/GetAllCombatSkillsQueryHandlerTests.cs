namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Fetch
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class GetAllCombatSkillsQueryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnAllFromSkillRespositoryWhenRequestIsHandled()
        {
            // Given
            var expected = new List<SkillInstance>();

            var skillRepositoryMock = new Mock<SkillRepository>();

            skillRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                expected
            );

            // When
            var handler = new GetAllCombatSkillsQueryHandler(
                skillRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new GetAllCombatSkillsQuery(),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
