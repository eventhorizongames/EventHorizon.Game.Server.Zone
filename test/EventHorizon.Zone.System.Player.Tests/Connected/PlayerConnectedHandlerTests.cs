using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.System.Player.Connected;
using EventHorizon.Zone.System.Player.Events.Connected;
using EventHorizon.Zone.System.Player.Events.Details;
using EventHorizon.Zone.System.Player.Model.Action;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Model.Position;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests.Connected
{
    public class PlayerConnectedHandlerTests
    {
        [Fact]
        public async Task TestShouldUpdatedPlayersConnectionIdWhenPlayerIsFoundInRepository()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var expectedPlayerAction = StandardPlayerAction.CONNECTION_ID;
            var expected = new PlayerEntity
            {
                PlayerId = playerId,
                ConnectionId = connectionId
            };
            var player = new PlayerEntity
            {
                PlayerId = playerId,
                ConnectionId = "old-connection-id"
            };

            var serverPropertyMock = new Mock<IServerProperty>();
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
            var handler = new PlayerConnectedHandler(
                mediatorMock.Object,
                serverPropertyMock.Object,
                playerRepositoryMock.Object
            );
            await handler.Handle(
                new PlayerConnectedEvent(
                    playerId,
                    connectionId
                ),
                CancellationToken.None
            );

            // Then
            playerRepositoryMock.Verify(
                mock => mock.Update(
                    expectedPlayerAction,
                    expected
                )
            );
        }

        [Fact]
        public async Task TestShouldRegisterPlayersWhenPlayerIsNotFoundInRepository()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var currentZoneServerId = "current-zone";
            var expectedPlayerAction = StandardPlayerAction.REGISTERED;
            var expected = new PlayerEntity
            {
                PlayerId = playerId,
                ConnectionId = connectionId
            };
            var registeredPlayer = new PlayerEntity
            {
                PlayerId = playerId,
                ConnectionId = null
            };
            var globalPlayerDetails = new PlayerDetails
            {
                Position = new PlayerPositionState
                {
                    CurrentZone = currentZoneServerId
                },
                Data = new Dictionary<string, object>()
            };

            var mediatorMock = new Mock<IMediator>();
            var serverPropertyMock = new Mock<IServerProperty>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RegisterEntityEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                registeredPlayer
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<PlayerGetDetailsEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                globalPlayerDetails
            );

            serverPropertyMock.Setup(
                mock => mock.Get<string>(
                    ServerPropertyKeys.SERVER_ID
                )
            ).Returns(
                currentZoneServerId
            );

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                default(PlayerEntity)
            );

            // When
            var handler = new PlayerConnectedHandler(
                mediatorMock.Object,
                serverPropertyMock.Object,
                playerRepositoryMock.Object
            );
            await handler.Handle(
                new PlayerConnectedEvent(
                    playerId,
                    connectionId
                ),
                CancellationToken.None
            );

            // Then
            playerRepositoryMock.Verify(
                mock => mock.Update(
                    expectedPlayerAction,
                    expected
                )
            );
        }

        [Fact]
        public async Task TestShouldThrowExceptionWhenCurrentZoneIsNotTheServerId()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var currentZoneServerId = "current-zone";
            var expected = "Player is not part of this server.";
            var globalPlayerDetails = new PlayerDetails
            {
                Position = new PlayerPositionState
                {
                    CurrentZone = "not-current-zone"
                },
                Data = new Dictionary<string, object>()
            };

            var mediatorMock = new Mock<IMediator>();
            var serverPropertyMock = new Mock<IServerProperty>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<PlayerGetDetailsEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                globalPlayerDetails
            );

            serverPropertyMock.Setup(
                mock => mock.Get<string>(
                    ServerPropertyKeys.SERVER_ID
                )
            ).Returns(
                currentZoneServerId
            );

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                default(PlayerEntity)
            );

            // When
            var handler = new PlayerConnectedHandler(
                mediatorMock.Object,
                serverPropertyMock.Object,
                playerRepositoryMock.Object
            );
            Func<Task> action = async () => await handler.Handle(
                new PlayerConnectedEvent(
                    playerId,
                    connectionId
                ),
                CancellationToken.None
            );

            // Then
            var exceptionDetails = await Assert.ThrowsAsync<Exception>(
                action
            );
            Assert.Equal(
                expected,
                exceptionDetails.Message
            );
            playerRepositoryMock.Verify(
                mock => mock.Update(
                    It.IsAny<EntityAction>(),
                    It.IsAny<PlayerEntity>()
                ),
                Times.Never()
            );
        }
    }
}