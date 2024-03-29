namespace EventHorizon.Zone.System.Player.Tests.Interaction.Run;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Interaction.Events;
using EventHorizon.Zone.System.Player.Events.Interaction.Run;
using EventHorizon.Zone.System.Player.Interaction.Run;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class RunEntityInteractionActionEventHandlerTests
{
    [Fact]
    public async Task ShouldRunInteractionCommandWhenOnPlayerInteractionEventIsProcessed()
    {
        // Given
        var interactionEntityId = 100L;
        var player = new PlayerEntity();
        var expected = new RunInteractionCommand(
            interactionEntityId,
            player
        );
        var inputEvent = (RunEntityInteractionActionEvent)new RunEntityInteractionActionEvent()
            .SetPlayer(
                player
            ).SetData(
                new Dictionary<string, object>
                {
                    { "interactionEntityId", interactionEntityId }
                }
            );

        var mediatorMock = new Mock<IMediator>();

        // When
        var handler = new RunEntityInteractionActionEventHandler(
            mediatorMock.Object
        );
        await handler.Handle(
            inputEvent,
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
