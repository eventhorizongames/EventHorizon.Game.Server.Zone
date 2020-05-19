namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.Entity.Model;
    using EventHorizon.Zone.Core.Entity.Search;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Events.Entity.Search;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class SearchInAreaWithTagHandlerTests
    {
        [Fact]
        public async Task ShouldCallIntoEntitySearchTreeWithEventParameters()
        {
            // Given
            var expectedSearchPositionCenter = new Vector3(2);
            var expectedSearchRadius = 32;
            var expectedTagList = new List<string>() { "player" };

            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            // When
            var searchInAreaHandler = new SearchInAreaWithTagHandler(
                entitySearchTreeMock.Object
            );

            var expectedEntityList = await searchInAreaHandler.Handle(
                new SearchInAreaWithTagEvent
                {
                    SearchPositionCenter = expectedSearchPositionCenter,
                    SearchRadius = expectedSearchRadius,
                    TagList = expectedTagList
                },
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.SearchInAreaWithTag(
                    expectedSearchPositionCenter,
                    expectedSearchRadius,
                    expectedTagList
                )
            );
        }

        [Fact]
        public async Task ShouldReturnEntityIdListFromEntitySearchTree()
        {
            // Given
            var searchPositionCenter = new Vector3(2);
            var searchRadius = 32;
            var tagList = new List<string>() { "player" };
            var entityId1 = 1;
            var entityId2 = 2;
            var expected = new List<int>
            {
                entityId1,
                entityId2,
            };
            var searchEntityList = new List<SearchEntity>()
            {
                new SearchEntity(
                    entityId1,
                    Vector3.Zero,
                    null
                ),
                new SearchEntity(
                    entityId2,
                    Vector3.Zero,
                    null
                ),
            };

            var entitySearchTreeMock = new Mock<EntitySearchTree>();
            entitySearchTreeMock.Setup(
                mock => mock.SearchInAreaWithTag(
                    searchPositionCenter,
                    searchRadius,
                    tagList
                )
            ).ReturnsAsync(
                searchEntityList
            );

            // When
            var SearchInAreaHandler = new SearchInAreaWithTagHandler(
                entitySearchTreeMock.Object
            );

            var actual = await SearchInAreaHandler.Handle(
                new SearchInAreaWithTagEvent
                {
                    SearchPositionCenter = searchPositionCenter,
                    SearchRadius = searchRadius,
                    TagList = tagList
                },
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldNotFailWhenSearchInAreaWithTagReturnsNull()
        {
            // Given
            var searchPositionCenter = new Vector3(2);
            var searchRadius = 32;
            var tagList = new List<string>() { "player" };
            List<SearchEntity> searchEntityList = null;

            var entitySearchTreeMock = new Mock<EntitySearchTree>();
            entitySearchTreeMock.Setup(
                mock => mock.SearchInAreaWithTag(
                    searchPositionCenter,
                    searchRadius,
                    tagList
                )
            ).ReturnsAsync(
                searchEntityList
            );

            // When
            var SearchInAreaHandler = new SearchInAreaWithTagHandler(
                entitySearchTreeMock.Object
            );

            var actual = await SearchInAreaHandler.Handle(
                new SearchInAreaWithTagEvent
                {
                    SearchPositionCenter = searchPositionCenter,
                    SearchRadius = searchRadius,
                    TagList = tagList
                },
                CancellationToken.None
            );

            // Then
            actual.Should().BeEmpty();
        }
    }
}