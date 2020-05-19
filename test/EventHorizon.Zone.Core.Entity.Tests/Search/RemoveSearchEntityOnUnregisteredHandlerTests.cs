namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.Entity.Model;
    using EventHorizon.Zone.Core.Entity.Search;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using Moq;
    using Xunit;

    public class RemoveSearchEntityOnUnregisteredHandlerTests
    {
        [Fact]
        public async Task TestShouldRemoveSearchEntityFromRepositoryBasedOnEvent()
        {
            // Given
            var entityId = 100L;
            var expected = new SearchEntity(
                entityId,
                Vector3.Zero,
                null
            );

            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var handler = new RemoveSearchEntityOnUnregisteredHandler(
                entitySearchTreeMock.Object
            );
            await handler.Handle(
                new EntityUnRegisteredEvent
                {
                    EntityId = entityId
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.Remove(
                    expected
                )
            );
        }
    }
}