using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Entity.Register.Handler;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Register;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Entity.Register;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Register.Handler
{
    public class UnregisterEntityHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldRemoveEntityByIdFromRepositoryThenPublishEntityUnregisteredEvent()
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
            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var unregisterEntityHandler = new UnregisterEntityHandler(
                mediatorMock.Object,
                entityRepositoryMock.Object
            );

            await unregisterEntityHandler.Handle(
                new UnRegisterEntityEvent
                {
                    Entity = expectedEntity.Object
                },
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