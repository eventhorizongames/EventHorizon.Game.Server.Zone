namespace EventHorizon.Zone.Core.Entity.Tests.Find
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Find;
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.Entity.State;

    using Moq;

    using Xunit;

    public class GetEntityByIdHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnFindByIdFromEntityRepository()
        {
            // Given
            var inputId = 123;
            var entityRepositoryMock = new Mock<EntityRepository>();

            // When
            var getEntityByIdHandler = new GetEntityByIdHandler(
                entityRepositoryMock.Object
            );
            await getEntityByIdHandler.Handle(
                new GetEntityByIdEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.FindById(
                    inputId
                )
            );
        }
    }
}
