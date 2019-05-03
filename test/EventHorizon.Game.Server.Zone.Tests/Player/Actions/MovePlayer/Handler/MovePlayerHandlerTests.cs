using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Model;
using System;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer.Handler;
using MediatR;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using System.Threading;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Game.Server.Zone.Model.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Player.Update;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.Player;

namespace EventHorizon.Game.Server.Zone.Tests.Player.Actions.MovePlayer.Handler
{
    public class MovePlayerHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnPlayerMoveToPositionWhenNextMoveRequestIsNotTime()
        {
            // Given
            var expected = new Vector3(100);
            var inputPlayer = new PlayerEntity
            {
                Position = new PositionState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.AddDays(1),
                    MoveToPosition = expected
                }
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(a => a.Now).Returns(DateTime.Now);
            var zoneMapFactoryMock = new Mock<IZoneMapFactory>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            // When
            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneMapFactoryMock.Object,
                playerRepositoryMock.Object
            );

            var actual = await movePlayerHandler.Handle(new MovePlayerEvent
            {
                Player = inputPlayer,
            }, CancellationToken.None);

            // Then
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(MoveDirections.Left, -5, 0)]
        [InlineData(MoveDirections.Right, 5, 0)]
        [InlineData(MoveDirections.Forward, 0, 5)]
        [InlineData(MoveDirections.Backwards, 0, -5)]
        [InlineData(9999, 0, 0)]
        public async Task TestHandle_ShouldReturnMoveDirection(long inputMoveDirection, float expectedXPosition, float expectedZPosition)
        {
            // Given
            var tileDimensions = 5;
            var currentPosition = Vector3.Zero;
            var playerMapNode = new MapNode(currentPosition);
            var zoneMap = new ZoneMap()
            {
                TileDimensions = tileDimensions
            };

            var inputPlayer = new PlayerEntity
            {
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = currentPosition,
                    NextMoveRequest = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                    MoveToPosition = new Vector3(0, 0, 0),
                }
            };

            var expected = new Vector3(expectedXPosition, 0, expectedZPosition);

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(a => a.Now).Returns(DateTime.Now);
            var zoneMapFactoryMock = new Mock<IZoneMapFactory>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            zoneMapFactoryMock.Setup(a => a.Map).Returns(zoneMap);

            mediatorMock.Setup(a => a.Send(new GetMapNodeAtPositionEvent
            {
                Position = currentPosition,
            }, CancellationToken.None)).ReturnsAsync(playerMapNode);

            // When
            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneMapFactoryMock.Object,
                playerRepositoryMock.Object
            );

            var actual = await movePlayerHandler.Handle(new MovePlayerEvent
            {
                Player = inputPlayer,
                MoveDirection = inputMoveDirection
            }, CancellationToken.None);

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestHandle_ShouldGlobalUpdatePlayer()
        {
            // Given
            var expectedEntityId = 321;

            var tileDimensions = 5;
            var currentPosition = Vector3.Zero;
            var playerMapNode = new MapNode(currentPosition);
            var zoneMap = new ZoneMap()
            {
                TileDimensions = tileDimensions
            };

            var inputPlayer = new PlayerEntity
            {
                Id = expectedEntityId,
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = currentPosition,
                    NextMoveRequest = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                    MoveToPosition = new Vector3(0, 0, 0),
                }
            };
            var inputMoveDirection = MoveDirections.Forward;

            var expectedCurrentPosition = new Vector3(0);
            var expectedMoveToPosition = new Vector3(0, 0, 5);

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(a => a.Now).Returns(DateTime.Now);
            var zoneMapFactoryMock = new Mock<IZoneMapFactory>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            zoneMapFactoryMock.Setup(a => a.Map).Returns(zoneMap);

            mediatorMock.Setup(a => a.Send(new GetMapNodeAtPositionEvent
            {
                Position = currentPosition,
            }, CancellationToken.None)).ReturnsAsync(playerMapNode);

            // When

            PlayerGlobalUpdateEvent actualUpdateEvent = new PlayerGlobalUpdateEvent
            {
                Player = inputPlayer
            };

            mediatorMock.Setup(a => a.Publish(It.IsAny<PlayerGlobalUpdateEvent>(), CancellationToken.None))
                .Callback<PlayerGlobalUpdateEvent, CancellationToken>((updateEvent, CancellationToken) =>
                {
                    actualUpdateEvent = updateEvent;
                }).Returns(Task.CompletedTask);

            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneMapFactoryMock.Object,
                playerRepositoryMock.Object
            );

            var actual = await movePlayerHandler.Handle(new MovePlayerEvent
            {
                Player = inputPlayer,
                MoveDirection = inputMoveDirection
            }, CancellationToken.None);

            // Then
            Assert.Equal(expectedCurrentPosition, actualUpdateEvent.Player.Position.CurrentPosition);
            Assert.Equal(expectedMoveToPosition, actualUpdateEvent.Player.Position.MoveToPosition);

            playerRepositoryMock.Verify(a => a.Update(PlayerAction.POSITION, actualUpdateEvent.Player));

            mediatorMock.Verify(a => a.Publish(new ClientActionEntityClientMoveToAllEvent
            {
                Data = new EntityClientMoveData
                {
                    EntityId = expectedEntityId,
                    MoveTo = expectedMoveToPosition
                }
            }, CancellationToken.None));
        }
    }
}