using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Entity.Register.Handler;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Entity.Register;

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