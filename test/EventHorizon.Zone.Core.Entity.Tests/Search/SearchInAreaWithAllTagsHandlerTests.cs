
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using Moq;
using Xunit;
using EventHorizon.Zone.Core.Events.Entity.Search;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Entity.Search;

namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    public class SearchInAreaWithAllTagsHandlerTests
    {
        [Fact]
        public async Task TestShouldCallIntoEntitySearchTreeWithEventParameters()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedTagList = new List<string>() { "player" };

            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var searchInAreaHandler = new SearchInAreaWithAllTagsHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await searchInAreaHandler.Handle(
                new SearchInAreaWithAllTagsEvent
                {
                    SearchPositionCenter = expectedSearchPositionCenter,
                    SearchRadius = expectedSearchRadius,
                    TagList = expectedTagList
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.SearchInAreaWithAllTags(
                    expectedSearchPositionCenter,
                    expectedSearchRadius,
                    expectedTagList
                )
            );
        }
        [Fact]
        public async Task TestShouldReturnEntityIdListFromEntitySearchTree()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedTagList = new List<string>() { "player" };
            var expectedEntityId1 = 1;
            var expectedEntityId2 = 2;
            var expectedSearchEntityList = new List<SearchEntity>()
            {
                new SearchEntity(
                    expectedEntityId1,
                    Vector3.Zero,
                    null
                ),
                new SearchEntity(
                    expectedEntityId2,
                    Vector3.Zero,
                    null
                ),
            };

            var entitySearchTreeMock = new Mock<EntitySearchTree>();
            entitySearchTreeMock.Setup(
                mock => mock.SearchInAreaWithAllTags(
                    expectedSearchPositionCenter,
                    expectedSearchRadius,
                    expectedTagList
                )
            ).ReturnsAsync(
                expectedSearchEntityList
            );

            // When
            var SearchInAreaHandler = new SearchInAreaWithAllTagsHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await SearchInAreaHandler.Handle(
                new SearchInAreaWithAllTagsEvent
                {
                    SearchPositionCenter = expectedSearchPositionCenter,
                    SearchRadius = expectedSearchRadius,
                    TagList = expectedTagList
                },
                CancellationToken.None
            );

            // Then
            Assert.Collection(
                expectedEntityList,
                entity => Assert.Equal(expectedEntityId1, entity),
                entity => Assert.Equal(expectedEntityId2, entity)
            );
        }
    }
}