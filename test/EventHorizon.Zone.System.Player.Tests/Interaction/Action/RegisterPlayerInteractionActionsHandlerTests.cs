namespace EventHorizon.Zone.System.Player.Tests.Interaction.Action;

using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.System.Player.Events.Interaction.Run;
using EventHorizon.Zone.System.Player.Interaction.Action;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class RegisterPlayerInteractionActionsHandlerTests
{
    [Fact]
    public async Task ShouldRegisterInteractionEventWhenReadyForPlayerActionRegistrationEventIsHandled()
    {
        // Given
        var nextId = 1L;
        var expected = new RegisterPlayerAction(
            nextId,
            PlayerInteractionActions.INTERACT,
            new RunEntityInteractionActionEvent()
        );

        var mediatorMock = new Mock<IMediator>();
        var idPoolMock = new Mock<IdPool>();

        idPoolMock.Setup(
            mock => mock.NextId()
        ).Returns(
            nextId
        );

        // When
        var handler = new RegisterPlayerInteractionActionsHandler(
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
