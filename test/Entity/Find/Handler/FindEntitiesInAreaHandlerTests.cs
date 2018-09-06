using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.State;
using System.Numerics;
using System.Threading;
using EventHorizon.Game.Server.Zone.Entity.Find;
using EventHorizon.Game.Server.Zone.Entity.Find.Handler;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Find.Handler
{
    public class FindEntitiesInAreaHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallIntoEntitySearchTreeWithEventParameters()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

            // When
            var findEntitiesInAreaHandler = new FindEntitiesInAreaHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await findEntitiesInAreaHandler.Handle(new FindEntitiesInAreaEvent
            {
                SearchPositionCenter = expectedSearchPositionCenter,
                SearchRadius = expectedSearchRadius
            }, CancellationToken.None);

            // Then
            entitySearchTreeMock.Verify(a => a.FindEntitiesInArea(expectedSearchPositionCenter, expectedSearchRadius));
        }
        [Fact]
        public async Task TestHandle_ShouldReturnEntityIdListFromEntitySearchTree()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedEntityId1 = 1;
            var expectedEntityId2 = 2;
            var expectedSearchEntityList = new List<SearchEntity>()
            {
                new SearchEntity(expectedEntityId1, Vector3.Zero, null),
                new SearchEntity(expectedEntityId2, Vector3.Zero, null),
            };

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();
            entitySearchTreeMock.Setup(a => a.FindEntitiesInArea(expectedSearchPositionCenter, expectedSearchRadius)).ReturnsAsync(expectedSearchEntityList);

            // When
            var findEntitiesInAreaHandler = new FindEntitiesInAreaHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await findEntitiesInAreaHandler.Handle(new FindEntitiesInAreaEvent
            {
                SearchPositionCenter = expectedSearchPositionCenter,
                SearchRadius = expectedSearchRadius
            }, CancellationToken.None);

            // Then
            Assert.Collection(expectedEntityList,
                a => Assert.Equal(expectedEntityId1, a),
                a => Assert.Equal(expectedEntityId2, a)
            );
        }
    }
}