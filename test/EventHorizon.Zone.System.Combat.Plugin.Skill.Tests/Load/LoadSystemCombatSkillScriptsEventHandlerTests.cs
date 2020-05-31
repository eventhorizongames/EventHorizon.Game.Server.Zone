namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Load
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class LoadSystemCombatSkillScriptsEventHandlerTests
    {
        [Fact]
        public async Task ShouldPublishLoadCombatSkillsEventWhenRequestIsHandled()
        {
            // Given
            var expected = new LoadCombatSkillEffectScripts();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new LoadSystemCombatSkillScriptsEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new LoadSystemCombatSkillScriptsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldPublishLoadSystemCombatSkillScriptsEventWhenRequestIsHandled()
        {
            // Given
            var expected = new LoadCombatSkillValidatorScripts();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new LoadSystemCombatSkillScriptsEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new LoadSystemCombatSkillScriptsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
