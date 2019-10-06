using Xunit;
using Moq;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Entity.Register;

namespace EventHorizon.Zone.Core.Entity.Tests.Register
{
    public class EntityUnregisteredHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishClientActionEvent()
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