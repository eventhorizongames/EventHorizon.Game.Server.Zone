using Xunit;
using Moq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Entity.Search;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Entity.Search;

namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    public class SearchInAreaHandlerTests
    {
        [Fact]
        public async Task TestShouldCallIntoEntitySearchTreeWithEventParameters()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;

            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var searchInAreaHandler = new SearchInAreaHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await searchInAreaHandler.Handle(
                new SearchInAreaEvent
                {
                    SearchPositionCenter = expectedSearchPositionCenter,
                    SearchRadius = expectedSearchRadius
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.SearchInArea(
                    expectedSearchPositionCenter,
                    expectedSearchRadius
                )
            );
        }
        [Fact]
        public async Task TestShouldReturnEntityIdListFromEntitySearchTree()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
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
                mock => mock.SearchInArea(
                    expectedSearchPositionCenter,
                    expectedSearchRadius
                )
            ).ReturnsAsync(
                expectedSearchEntityList
            );

            // When
            var SearchInAreaHandler = new SearchInAreaHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await SearchInAreaHandler.Handle(
                new SearchInAreaEvent
                {
                    SearchPositionCenter = expectedSearchPositionCenter,
                    SearchRadius = expectedSearchRadius
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