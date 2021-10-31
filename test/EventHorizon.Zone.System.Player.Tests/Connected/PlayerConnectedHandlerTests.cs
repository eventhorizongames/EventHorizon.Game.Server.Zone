namespace EventHorizon.Zone.System.Player.Tests.Connected
{
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Exceptions;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.System.Player.Connected;
    using EventHorizon.Zone.System.Player.Events.Connected;
    using EventHorizon.Zone.System.Player.Events.Details;
    using EventHorizon.Zone.System.Player.Model.Action;
    using EventHorizon.Zone.System.Player.Model.Details;
    using EventHorizon.Zone.System.Player.Set;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class PlayerConnectedHandlerTests
    {
        [Fact]
        public async Task ShouldUpdatedPlayersConnectionIdWhenPlayerIsFoundInRepository()
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
            mediatorMock.Verify(
                mock => mock.Send(
                    new UpdateEntityCommand(
                        expectedPlayerAction,
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldRegisterPlayersWhenPlayerIsNotFoundInRepository()
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
                Location = new LocationState
                {
                    CurrentZone = currentZoneServerId
                },
                Data = new ConcurrentDictionary<string, object>(),
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

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SetPlayerPropertyOverrideDataCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<PlayerEntity>(
                    registeredPlayer
                )
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
            mediatorMock.Verify(
                mock => mock.Send(
                    new UpdateEntityCommand(
                        expectedPlayerAction,
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenCurrentZoneIsNotTheServerId()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var currentZoneServerId = "current-zone";
            var errorCode = "player_not_part_of_server";
            var expected = new PlatformErrorCodeException(
                errorCode,
                "Player is not part of this server."
            );
            var globalPlayerDetails = new PlayerDetails
            {
                Location = new LocationState
                {
                    CurrentZone = "not-current-zone"
                },
                Data = new ConcurrentDictionary<string, object>()
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
            async Task action() => await handler.Handle(
                new PlayerConnectedEvent(
                    playerId,
                    connectionId
                ),
                CancellationToken.None
            );

            // Then
            var exceptionDetails = await Assert.ThrowsAsync<PlatformErrorCodeException>(
                action
            );
            exceptionDetails.Message
                .Should().Be(expected.Message);
            exceptionDetails.ErrorCode
                .Should().Be(errorCode);
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<UpdateEntityCommand>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenCurrentZoneIsNotTheServerIdIsNull()
        {
            // Given
            var playerId = "player-id";
            var connectionId = "connection-id";
            var currentZoneServerId = "current-zone";
            var errorCode = "player_not_part_of_server";
            var expected = new PlatformErrorCodeException(
                errorCode,
                "Player is not part of this server."
            );
            var globalPlayerDetails = new PlayerDetails
            {
                Location = new LocationState
                {
                    CurrentZone = null,
                },
                Data = new ConcurrentDictionary<string, object>()
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
            async Task action() => await handler.Handle(
                new PlayerConnectedEvent(
                    playerId,
                    connectionId
                ),
                CancellationToken.None
            );

            // Then
            var exceptionDetails = await Assert.ThrowsAsync<PlatformErrorCodeException>(
                action
            );
            exceptionDetails.Message
                .Should().Be(expected.Message);
            exceptionDetails.ErrorCode
                .Should().Be(errorCode);
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<UpdateEntityCommand>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }
    }
}
