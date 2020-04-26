namespace EventHorizon.Zone.Core.Entity.Tests.Register
{
    using EventHorizon.Zone.Core.Entity.Register;
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using MediatR;
    using Moq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

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
                    ClientActionEntityUnregisteredToAllEvent.Create(
                        new EntityUnregisteredData
                        {
                            EntityId = expectedEntityId
                        }
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}