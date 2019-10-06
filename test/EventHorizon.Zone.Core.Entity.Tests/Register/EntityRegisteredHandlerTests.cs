using Xunit;
using Moq;
using MediatR;
using EventHorizon.Zone.Core.Model.Client.DataType;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Entity.Register;

namespace EventHorizon.Zone.Core.Entity.Tests.Register
{
    public class EntityRegisteredHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishClientActionEvent()
        {
            // Given
            var expectedEntity = new Mock<IObjectEntity>();

            var mediatorMock = new Mock<IMediator>();

            // When
            var entityRegisteredHandler = new EntityRegisteredHandler(
                mediatorMock.Object
            );

            await entityRegisteredHandler.Handle(
                new EntityRegisteredEvent
                {
                    Entity = expectedEntity.Object
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new ClientActionEntityRegisteredToAllEvent
                    {
                        Data = new EntityRegisteredData
                        {
                            Entity = expectedEntity.Object
                        }
                    },
                    CancellationToken.None
                )
            );
        }
    }
}