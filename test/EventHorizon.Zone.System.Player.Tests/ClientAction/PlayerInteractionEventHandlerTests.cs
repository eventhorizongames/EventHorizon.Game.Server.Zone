using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Interaction.Events;
using EventHorizon.Zone.System.Player.ClientAction;
using EventHorizon.Zone.System.Player.Events.ClientAction;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests.ClientAction
{
    public class PlayerInteractionEventHandlerTests
    {
        [Fact]
        public async Task TestShouldRunInteractionCommandWhenOnPlayerInteractionEventIsProcessed()
        {
            // Given
            var interactionEntityId = 100L;
            var player = new PlayerEntity();
            var expected = new RunInteractionCommand(
                interactionEntityId,
                player
            );

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new PlayerInteractionEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new PlayerInteractionEvent(
                    player,
                    interactionEntityId
                ),
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