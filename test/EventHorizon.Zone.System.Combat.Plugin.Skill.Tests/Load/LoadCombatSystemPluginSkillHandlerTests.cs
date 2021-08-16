namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Load
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadCombatSystemPluginSkillHandlerTests
    {
        [Fact]
        public async Task ShouldPublishLoadCombatSkillsEventWhenRequestIsHandled()
        {
            // Given
            var expected = new LoadCombatSkillsEvent();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new LoadSystemCombatPluginSkillHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new LoadSystemCombatPluginSkill(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
