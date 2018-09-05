using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Find.Handler;
using EventHorizon.Game.Server.Zone.Entity.Find;
using System.Threading.Tasks;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Find.Handler
{
    public class GetEntityByIdHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnFindByIdFromEntityRepository()
        {
            // Given
            var inputId = 123;
            var entityRepositoryMock = new Mock<IEntityRepository>();

            // When
            var getEntityByIdHandler = new GetEntityByIdHandler(
                entityRepositoryMock.Object
            );
            await getEntityByIdHandler.Handle(new GetEntityByIdEvent
            {
                EntityId = inputId
            }, CancellationToken.None);

            // Then
            entityRepositoryMock.Verify(a => a.FindById(inputId));
        }
    }
}