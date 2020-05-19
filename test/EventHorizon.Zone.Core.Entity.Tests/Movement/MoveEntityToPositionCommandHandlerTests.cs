namespace EventHorizon.Zone.Core.Entity.Tests.Movement
{
    using System;
    using System.Collections.Concurrent;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Map;
    using EventHorizon.Zone.Core.Model.Settings;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using Xunit;

    public class MoveEntityToPositionCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnFailureAndErrorMessageWhenMoveToMapNodeIsDenseAndDoDensityCheckIsTrue()
        {
            // Given
            var expected = new MoveEntityToPositionCommandResponse(
                false,
                "move_to_node_is_dense"
            );

            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            var moveTo = Vector3.One;
            var doDensityCheck = true;

            var moveToMapNode = new MapNode(
                moveTo
            );
            moveToMapNode.Info["dense"] = 1;

            var zoneSettings = new ZoneSettings();

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        moveTo
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                moveToMapNode
            );

            // When
            var handler = new MoveEntityToPositionCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneSettings
            );
            var actual = await handler.Handle(
                new MoveEntityToPositionCommand(
                    entity,
                    moveTo,
                    doDensityCheck
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnFailureAndErrorMessageWhenMoveToMapNodeIsNotFound()
        {
            // Given
            var expected = new MoveEntityToPositionCommandResponse(
                false,
                "move_to_node_is_dense"
            );

            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            var moveTo = Vector3.One;
            var doDensityCheck = true;

            var zoneSettings = new ZoneSettings();

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        moveTo
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                default(MapNode)
            );

            // When
            var handler = new MoveEntityToPositionCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneSettings
            );
            var actual = await handler.Handle(
                new MoveEntityToPositionCommand(
                    entity,
                    moveTo,
                    doDensityCheck
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnSuccesssWhenMoveToMapNodeIsNotDenseAndDoDensityCheckIsTrue()
        {
            // Given
            var now = DateTime.UtcNow;
            var moveTo = Vector3.One;
            var doDensityCheck = true;
            var baseMovementTimeOffset = 10;
            var speed = 1;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            entity.PopulateData(
                LocationState.PROPERTY_NAME,
                new LocationState()
            );
            entity.PopulateData(
                MovementState.PROPERTY_NAME,
                new MovementState
                {
                    Speed = speed,
                }
            );

            var zoneSettings = new ZoneSettings
            {
                BaseMovementTimeOffset = baseMovementTimeOffset,
            };

            var moveToMapNode = new MapNode(
                moveTo
            );
            moveToMapNode.Info["dense"] = 0;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        moveTo
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                moveToMapNode
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new MoveEntityToPositionCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneSettings
            );
            var actual = await handler.Handle(
                new MoveEntityToPositionCommand(
                    entity,
                    moveTo,
                    doDensityCheck
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccesssWhenMoveToMapNodeDoesNotContainDenseInfo()
        {
            // Given
            var now = DateTime.UtcNow;
            var moveTo = Vector3.One;
            var doDensityCheck = true;
            var baseMovementTimeOffset = 10;
            var speed = 1;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            entity.PopulateData(
                LocationState.PROPERTY_NAME,
                new LocationState()
            );
            entity.PopulateData(
                MovementState.PROPERTY_NAME,
                new MovementState
                {
                    Speed = speed,
                }
            );

            var zoneSettings = new ZoneSettings
            {
                BaseMovementTimeOffset = baseMovementTimeOffset,
            };

            var moveToMapNode = new MapNode(
                moveTo
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        moveTo
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                moveToMapNode
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new MoveEntityToPositionCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneSettings
            );
            var actual = await handler.Handle(
                new MoveEntityToPositionCommand(
                    entity,
                    moveTo,
                    doDensityCheck
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldUpdateTheEntitysLocationStateWhenPassingDensityCheck()
        {
            // Given
            IObjectEntity actual = null;

            var now = DateTime.UtcNow;
            var moveTo = Vector3.One;
            var doDensityCheck = false;
            var baseMovementTimeOffset = 10;
            var speed = 1;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            entity.PopulateData(
                LocationState.PROPERTY_NAME,
                new LocationState()
            );
            entity.PopulateData(
                MovementState.PROPERTY_NAME,
                new MovementState
                {
                    Speed = speed,
                }
            );
            var expectedPosition = moveTo;
            var expectedMoveToPosition = moveTo;
            // 10 * 1 / 1
            var expectedNextMoveRequest = now.AddMilliseconds(
                10
            );

            var zoneSettings = new ZoneSettings
            {
                BaseMovementTimeOffset = baseMovementTimeOffset,
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<UpdateEntityCommand>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (request, _) =>
                {
                    actual = ((UpdateEntityCommand)request).Entity;
                }
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new MoveEntityToPositionCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                zoneSettings
            );
            await handler.Handle(
                new MoveEntityToPositionCommand(
                    entity,
                    moveTo,
                    doDensityCheck
                ),
                CancellationToken.None
            );

            // Then
            actual
                .Should().NotBeNull();
            actual.Transform.Position
                .Should().Be(expectedPosition);
            var actualLocationState = actual.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            actualLocationState.MoveToPosition
                .Should().Be(expectedMoveToPosition);
            actualLocationState.NextMoveRequest
                .Should().Be(expectedNextMoveRequest);
        }
    }
}
