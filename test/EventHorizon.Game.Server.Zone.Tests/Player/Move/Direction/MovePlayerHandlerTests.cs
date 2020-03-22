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
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Player.Move.Direction
{
    public class MovePlayerHandlerTests
    {
        [Fact]
        public async Task TestShouldIgnorePlayerMoveToPositionWhenNextMoveRequestIsNotTime()
        {
            // Given
            var expected = new Vector3(100);
            var inputPlayer = new PlayerEntity();
            inputPlayer.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.AddDays(1),
                    MoveToPosition = expected
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var mapDetailsMock = new Mock<IMapDetails>();

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                DateTime.Now
            );

            // When
            var movePlayerHandler = new MovePlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                mapDetailsMock.Object
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

            var inputPlayer = new PlayerEntity()
            {
                Transform = new TransformState
                {
                    Position = currentPosition,
                },
                RawData = new Dictionary<string, object>(),
            };
            inputPlayer.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = true,
                    NextMoveRequest = DateTime.Now.Subtract(
                        TimeSpan.FromDays(1)
                    ),
                    MoveToPosition = new Vector3(
                        0,
                        0,
                        0
                    ),
                }
            );

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
                mapDetailsMock.Object
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
            mediatorMock.Verify(
                mock => mock.Send(
                    new MoveEntityToPositionCommand(
                        inputPlayer,
                        moveToPostion,
                        true
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}