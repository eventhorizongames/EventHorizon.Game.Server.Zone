using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Connected;
using EventHorizon.Zone.System.Player.Events.Connected;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests.Connected
{
    public class PlayerDisconnectedHandlerTests
    {
        [Fact]
        public async Task TestShouldUnRegisterPlayerWhenPlayerIsFound()
        {
            // Given
            var playerId = "player-id";
            var player = new PlayerEntity
            {
                PlayerId = playerId
            };
            var expected = new UnRegisterEntityEvent(
                player
            );

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );

            // When
            var handler = new PlayerDisconnectedHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object
            );

            await handler.Handle(
                new PlayerDisconnectedEvent(
                    playerId
                ),
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
        [Fact]
        public async Task TestShouldNotUnRegisterPlayerWhenPlayerIsNotFound()
        {
            // Given
            var playerId = "player-id";

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                default(PlayerEntity)
            );

            // When
            var handler = new PlayerDisconnectedHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object
            );

            await handler.Handle(
                new PlayerDisconnectedEvent(
                    playerId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<UnRegisterEntityEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}