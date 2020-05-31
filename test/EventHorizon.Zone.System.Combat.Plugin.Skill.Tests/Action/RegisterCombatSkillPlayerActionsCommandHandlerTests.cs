namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Action
{
    using EventHorizon.Zone.Core.Model.Id;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Action;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;
    using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class RegisterCombatSkillPlayerActionsCommandHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterPlayerActionWhenNotificationIsHandled()
        {
            // Given
            var nextId = 123L;
            var expected = new RegisterPlayerAction(
                nextId,
                PlayerSkillActions.RUN_SKILL,
                new RunSkillWithTargetOfEntityEvent()
            );

            var mediatorMock = new Mock<IMediator>();
            var idPoolMock = new Mock<IdPool>();

            idPoolMock.Setup(
                mock => mock.NextId()
            ).Returns(
                nextId
            );

            // When
            var handler = new RegisterCombatSkillPlayerActionsCommandHandler(
                mediatorMock.Object,
                idPoolMock.Object
            );
            await handler.Handle(
                new ReadyForPlayerActionRegistration(),
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
