namespace EventHorizon.Zone.Core.Entity.Tests.Update
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Entity.Update;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using Moq;
    using Xunit;

    public class UpdateEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateEntityInRepository()
        {
            // Given
            var action = EntityAction.ADD;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            var entityRepositoryMock = new Mock<EntityRepository>();

            // When
            var handler = new UpdateEntityCommandHandler(
                entityRepositoryMock.Object
            );
            await handler.Handle(
                new UpdateEntityCommand(
                    action,
                    entity
                ),
                CancellationToken.None
            );

            // Then
            entityRepositoryMock.Verify(
                mock => mock.Update(
                    action,
                    entity
                )
            );
        }
    }
}
