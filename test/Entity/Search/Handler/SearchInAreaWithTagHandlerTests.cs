
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Search;
using EventHorizon.Game.Server.Zone.Entity.Search.Handler;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Search.Handler
{
    public class SearchInAreaWithTagHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallIntoEntitySearchTreeWithEventParameters()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedTagList = new List<string>() { "player" };

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

            // When
            var SearchInAreaHandler = new SearchInAreaWithTagHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await SearchInAreaHandler.Handle(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = expectedSearchPositionCenter,
                SearchRadius = expectedSearchRadius,
                TagList = expectedTagList
            }, CancellationToken.None);

            // Then
            entitySearchTreeMock.Verify(a => a.SearchInAreaWithTag(expectedSearchPositionCenter, expectedSearchRadius, expectedTagList));
        }
        [Fact]
        public async Task TestHandle_ShouldReturnEntityIdListFromEntitySearchTree()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedTagList = new List<string>() { "player" };
            var expectedEntityId1 = 1;
            var expectedEntityId2 = 2;
            var expectedSearchEntityList = new List<SearchEntity>()
            {
                new SearchEntity(expectedEntityId1, Vector3.Zero, null),
                new SearchEntity(expectedEntityId2, Vector3.Zero, null),
            };

            var entitySearchTreeMock = new Mock<IEntitySearchTree>();
            entitySearchTreeMock.Setup(a => a.SearchInAreaWithTag(expectedSearchPositionCenter, expectedSearchRadius, expectedTagList)).ReturnsAsync(expectedSearchEntityList);

            // When
            var SearchInAreaHandler = new SearchInAreaWithTagHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await SearchInAreaHandler.Handle(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = expectedSearchPositionCenter,
                SearchRadius = expectedSearchRadius,
                TagList = expectedTagList
            }, CancellationToken.None);

            // Then
            Assert.Collection(expectedEntityList,
                a => Assert.Equal(expectedEntityId1, a),
                a => Assert.Equal(expectedEntityId2, a)
            );
        }
    }
}