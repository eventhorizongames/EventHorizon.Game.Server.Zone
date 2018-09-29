using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Register.Handler;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Entity.Model;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.External.Entity;

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
            expectedEntity.Setup(a => a.Id).Returns(expectedId);

            var mediatorMock = new Mock<IMediator>();
            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var unregisterEntityHandler = new UnregisterEntityHandler(
                mediatorMock.Object,
                entityRepositoryMock.Object
            );

            await unregisterEntityHandler.Handle(new UnregisterEntityEvent
            {
                Entity = expectedEntity.Object
            }, CancellationToken.None);

            // Then
            entityRepositoryMock.Verify(a => a.Remove(expectedId));
            mediatorMock.Verify(a => a.Publish(new EntityUnregisteredEvent
            {
                EntityId = expectedId
            }, CancellationToken.None));
        }
    }
}