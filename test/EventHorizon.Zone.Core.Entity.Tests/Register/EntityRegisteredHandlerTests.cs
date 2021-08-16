namespace EventHorizon.Zone.Core.Entity.Tests.Register
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Register;
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    using MediatR;

    using Moq;

    using Xunit;

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
                    ClientActionEntityRegisteredToAllEvent.Create(
                        new EntityRegisteredData
                        {
                            Entity = expectedEntity.Object
                        }
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
