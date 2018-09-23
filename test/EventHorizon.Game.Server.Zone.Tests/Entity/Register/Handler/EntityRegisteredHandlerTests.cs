using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Client.DataType;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Registered.Handler;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Register.Handler
{
    public class EntityRegisteredHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishClientActionEvent()
        {
            // Given
            var expectedEntity = new Mock<IObjectEntity>();

            var mediatorMock = new Mock<IMediator>();

            // When
            var entityRegisteredHandler = new EntityRegisteredHandler(
                mediatorMock.Object
            );

            await entityRegisteredHandler.Handle(new EntityRegisteredEvent
            {
                Entity = expectedEntity.Object
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(new ClientActionEvent
            {
                Action = "EntityRegistered",
                Data = new EntityRegisteredData
                {
                    Entity = expectedEntity.Object
                }
            }, CancellationToken.None));
        }
    }
}