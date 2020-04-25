namespace EventHorizon.Zone.Core.Entity.Tests.Movement
{
    using EventHorizon.Zone.Core.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using Moq;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class EntityCanMoveHandlerTests
    {
        [Fact]
        public async Task TestShouldNotUpdateEntityWhenNotFoundInRepository()
        {
            // Given
            var entityRepositoryMock = new Mock<EntityRepository>();
            entityRepositoryMock.Setup(
                mock => mock.FindById(
                    It.IsAny<long>()
                )
            ).ReturnsAsync(
                DefaultEntity.NULL
            );

            // When
            var handler = new EntityCanMoveHandler(
                entityRepositoryMock.Object
            );
            await handler.Handle(
                new EntityCanMoveEvent
                {
                    EntityId = 1L
                },
                CancellationToken.None
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.Update(
                    It.IsAny<EntityAction>(),
                    It.IsAny<IObjectEntity>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldUpdateEntityToCanMoveToTrueWhenFoundInRepository()
        {
            // Given
            var entityId = 100L;
            var expected = new Model.Core.LocationState
            {
                CanMove = true
            };
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = entityId,
            };
            entity.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                new LocationState
                {
                    CanMove = false
                }
            );

            var entityRepositoryMock = new Mock<EntityRepository>();
            entityRepositoryMock.Setup(
                mock => mock.FindById(
                    entityId
                )
            ).ReturnsAsync(
                entity
            );

            // When
            var handler = new EntityCanMoveHandler(
                entityRepositoryMock.Object
            );
            await handler.Handle(
                new EntityCanMoveEvent
                {
                    EntityId = entityId
                },
                CancellationToken.None
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.Update(
                    EntityAction.PROPERTY_CHANGED,
                    It.Is<IObjectEntity>(
                        actualEntity => actualEntity.GetProperty<LocationState>(
                            LocationState.PROPERTY_NAME
                        ).Equals(
                            expected
                        )
                    )
                )
            );
        }
    }
}