namespace EventHorizon.Game.Tests.Capture
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Capture;
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    using Moq;

    using Xunit;

    public class RunCaptureLogicForAllPlayersHandlerTests
    {
        [Fact]
        public async Task ShouldTriggerRunCaptureLogicForPlayerWhenEventIsHandled()
        {
            // Given
            var playerEntityId = 123L;
            var playerList = new List<IObjectEntity>
            {
                new PlayerEntity
                {
                    Id = playerEntityId,
                    Type = EntityType.PLAYER,
                },
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<QueryForEntities>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (QueryForEntities request, CancellationToken _) =>
                {
                    return playerList.Where(request.Query);
                }
            );

            // When
            var handler = new RunCaptureLogicForAllPlayersHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForAllPlayers(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RunCaptureLogicForPlayer(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            );

        }
    }
}
