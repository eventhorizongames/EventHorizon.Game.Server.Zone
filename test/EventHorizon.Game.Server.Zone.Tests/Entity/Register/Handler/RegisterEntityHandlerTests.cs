using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Register.Handler;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Register;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.External.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Register.Handler
{
    public class RegisterEntityHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldAddEntityToRepositoryThenPublishEntityRegisteredEvent()
        {
            // Given
            var expectedEntity = new Mock<IObjectEntity>();

            var mediatorMock = new Mock<IMediator>();
            var entityRepositoryMock = new Mock<IEntityRepository>();

            entityRepositoryMock.Setup(a => a.Add(expectedEntity.Object))
                .ReturnsAsync(expectedEntity.Object);
            // When
            var registerEntityHandler = new RegisterEntityHandler(
                mediatorMock.Object,
                entityRepositoryMock.Object
            );

            await registerEntityHandler.Handle(new RegisterEntityEvent
            {
                Entity = expectedEntity.Object
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(new EntityRegisteredEvent
            {
                Entity = expectedEntity.Object
            }, CancellationToken.None));
        }
    }
}