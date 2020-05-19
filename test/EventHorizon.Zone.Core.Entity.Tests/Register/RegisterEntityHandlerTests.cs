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

    public class RegisterEntityHandlerTests
    {
        [Fact]
        public async Task TestShouldAddEntityToRepositoryThenPublishEntityRegisteredEvent()
        {
            // Given
            var expectedEntity = new Mock<IObjectEntity>();

            var mediatorMock = new Mock<IMediator>();
            var entityRepositoryMock = new Mock<EntityRepository>();

            entityRepositoryMock.Setup(
                mock => mock.Add(
                    expectedEntity.Object
                )
            ).ReturnsAsync(
                expectedEntity.Object
            );

            // When
            var registerEntityHandler = new RegisterEntityHandler(
                mediatorMock.Object,
                entityRepositoryMock.Object
            );

            await registerEntityHandler.Handle(
                new RegisterEntityEvent
                {
                    Entity = expectedEntity.Object
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new EntityRegisteredEvent
                    {
                        Entity = expectedEntity.Object
                    },
                    CancellationToken.None
                )
            );
        }
    }
}