namespace EventHorizon.Zone.System.Player.Tests.Update
{
    using EventHorizon.Zone.Core.Events.Entity.Action;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Events.Update;
    using EventHorizon.Zone.System.Player.Update;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class PlayerEntityActionEventHandlerTests
    {
        [Fact]
        public async Task ShouldNotUpdateGlobalPlayerWhenEntityIsNotPlayerTypeOrPlayerEntity()
        {
            // Given
            var entity = new DefaultEntity();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new PlayerEntityActionEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityActionEvent(
                    EntityAction.PROPERTY_CHANGED,
                    entity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerGlobalUpdateEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldNotUpdateGlobalPlayerWhenEntityIsNotPlayerType()
        {
            // Given
            var entity = new PlayerEntity
            {
                Type = EntityType.OTHER,
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new PlayerEntityActionEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityActionEvent(
                    EntityAction.PROPERTY_CHANGED,
                    entity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerGlobalUpdateEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldUpdateGlobalPlayerWhenEntityIsPlayerTypeAndPlayerEntity()
        {
            // Given
            var playerEntity = new PlayerEntity
            {
                Type = EntityType.PLAYER,
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new PlayerEntityActionEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityActionEvent(
                    EntityAction.PROPERTY_CHANGED,
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new PlayerGlobalUpdateEvent(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
