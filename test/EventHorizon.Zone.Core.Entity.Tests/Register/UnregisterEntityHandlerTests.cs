namespace EventHorizon.Zone.Core.Entity.Tests.Register
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Entity.Register;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using MediatR;
    using Moq;
    using Xunit;

    public class UnregisterEntityHandlerTests
    {
        [Fact]
        public async Task TestShouldRemoveEntityByIdFromRepositoryThenPublishEntityUnregisteredEvent()
        {
            // Given
            var expectedId = 123;
            var expectedEntity = new Mock<IObjectEntity>();
            expectedEntity.Setup(
                mock => mock.Id
            ).Returns(
                expectedId
            );

            var mediatorMock = new Mock<IMediator>();
            var entityRepositoryMock = new Mock<EntityRepository>();

            // When
            var unregisterEntityHandler = new UnregisterEntityHandler(
                mediatorMock.Object,
                entityRepositoryMock.Object
            );

            await unregisterEntityHandler.Handle(
                new UnRegisterEntityEvent(
                    expectedEntity.Object
                ),
                CancellationToken.None
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.Remove(
                    expectedId
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new EntityUnRegisteredEvent
                    {
                        EntityId = expectedId
                    },
                    CancellationToken.None
                )
            );
        }
    }
}