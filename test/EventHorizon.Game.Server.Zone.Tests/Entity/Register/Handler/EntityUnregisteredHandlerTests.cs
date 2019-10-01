using Xunit;
using Moq;
using System.Threading.Tasks;
using MediatR;
using EventHorizon.Game.Server.Zone.Entity.Registered.Handler;
using System.Threading;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Entity.Register;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Register.Handler
{
    public class EntityUnregisteredHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishClientActionEvent()
        {
            // Given
            var expectedEntityId = 123;

            var mediatorMock = new Mock<IMediator>();

            // When
            var entityRegisteredHandler = new EntityUnregisteredHandler(
                mediatorMock.Object
            );

            await entityRegisteredHandler.Handle(
                new EntityUnRegisteredEvent
                {
                    EntityId = expectedEntityId
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new ClientActionEntityUnregisteredToAllEvent
                    {
                        Data = new EntityUnregisteredData
                        {
                            EntityId = expectedEntityId
                        }
                    },
                    CancellationToken.None
                )
            );
        }
    }
}