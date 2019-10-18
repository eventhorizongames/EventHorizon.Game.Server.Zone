using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Model;
using System;
using System.Numerics;
using MediatR;
using System.Threading;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.System.Player.Events.Update;
using EventHorizon.Game.Server.Zone.Player.Action.Direction;
using EventHorizon.Game.Server.Zone.Player.Move.Model;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Player.Move.Direction;

namespace EventHorizon.Game.Server.Zone.Tests.Player.Move.Direction
{
    public class MovePlayerHandlerTests
    {
        [Fact]
        public async Task TestShouldIgnorePlayerMoveToPositionWhenNextMoveRequestIsNotTime()
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
            var mapDetailsMock = new Mock<IMapDetails>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                DateTime.Now
            );

            // When
            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                mapDetailsMock.Object,
                playerRepositoryMock.Object
            );

            await movePlayerHandler.Handle(
                new MovePlayerEvent
                {
                    Player = inputPlayer,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<GetMapNodeAtPositionEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Theory]
        [InlineData(MoveDirections.Left, -5, 0)]
        [InlineData(MoveDirections.Right, 5, 0)]
        [InlineData(MoveDirections.Forward, 0, 5)]
        [InlineData(MoveDirections.Backwards, 0, -5)]
        [InlineData(9999, 0, 0)]
        public async Task TestShouldReturnMoveDirection(
            long inputMoveDirection,
            float expectedXPosition,
            float expectedZPosition
        )
        {
            // Given
            var tileDimensions = 5;
            var currentPosition = Vector3.Zero;
            var playerMapNode = new MapNode(
                currentPosition
            );
            var moveToPostion = new Vector3(
                expectedXPosition,
                0,
                expectedZPosition
            );

            var inputPlayer = new PlayerEntity
            {
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = currentPosition,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromDays(1)
                    ),
                    MoveToPosition = new Vector3(
                        0,
                        0,
                        0
                    ),
                }
            };

            var expected = new Vector3(
                expectedXPosition,
                0,
                expectedZPosition
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                DateTime.Now
            );
            var mapDetailsMock = new Mock<IMapDetails>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            mapDetailsMock.Setup(
                mock => mock.TileDimensions
            ).Returns(
                tileDimensions
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent
                    {
                        Position = moveToPostion,
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerMapNode
            );

            // When
            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                mapDetailsMock.Object,
                playerRepositoryMock.Object
            );

            await movePlayerHandler.Handle(
                new MovePlayerEvent
                {
                    Player = inputPlayer,
                    MoveDirection = inputMoveDirection
                },
                CancellationToken.None
            );

            // Then
            // Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestShouldGlobalUpdatePlayer()
        {
            // Given
            var inputMoveDirection = MoveDirections.Forward;
            var expectedEntityId = 321;

            var tileDimensions = 5;
            var currentPosition = Vector3.Zero;
            var playerMapNode = new MapNode(
                currentPosition
            );
            var moveToPostion = new Vector3(
                0,
                0,
                tileDimensions
            );

            var inputPlayer = new PlayerEntity
            {
                Id = expectedEntityId,
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = currentPosition,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromDays(1)
                    ),
                    MoveToPosition = new Vector3(
                        0,
                        0,
                        0
                    ),
                }
            };

            var expectedCurrentPosition = new Vector3(0);
            var expectedMoveToPosition = new Vector3(
                0,
                0,
                5
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                DateTime.Now
            );
            var mapDetailsMock = new Mock<IMapDetails>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();

            mapDetailsMock.Setup(
                mock => mock.TileDimensions
            ).Returns(
                tileDimensions
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent
                    {
                        Position = moveToPostion,
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerMapNode
            );

            // When
            var actualUpdateEvent = new PlayerGlobalUpdateEvent(
                inputPlayer
            );

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<PlayerGlobalUpdateEvent>(),
                    CancellationToken.None
                )
            ).Callback<PlayerGlobalUpdateEvent, CancellationToken>(
                (updateEvent, CancellationToken) =>
                {
                    actualUpdateEvent = updateEvent;
                }
            ).Returns(
                Task.CompletedTask
            );

            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                mapDetailsMock.Object,
                playerRepositoryMock.Object
            );

            await movePlayerHandler.Handle(
                new MovePlayerEvent
                {
                    Player = inputPlayer,
                    MoveDirection = inputMoveDirection
                },
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expectedCurrentPosition,
                actualUpdateEvent.Player.Position.CurrentPosition
            );
            Assert.Equal(
                expectedMoveToPosition,
                actualUpdateEvent.Player.Position.MoveToPosition
            );

            playerRepositoryMock.Verify(
                mock => mock.Update(
                    PlayerAction.POSITION,
                    actualUpdateEvent.Player
                )
            );

            mediatorMock.Verify(
                mock => mock.Publish(
                    new ClientActionEntityClientMoveToAllEvent
                    {
                        Data = new EntityClientMoveData
                        {
                            EntityId = expectedEntityId,
                            MoveTo = expectedMoveToPosition
                        }
                    },
                    CancellationToken.None
                )
            );
        }
    }
}