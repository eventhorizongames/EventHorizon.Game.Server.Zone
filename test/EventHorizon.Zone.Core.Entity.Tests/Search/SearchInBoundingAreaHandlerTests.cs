using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.Search;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Search;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    public class SearchInBoundingAreaHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnListOfEntityIdsFromTheEntitySearchTree()
        {
            // Given
            var expected = 1L;
            var inputCenterPosition = Vector3.Zero;
            var inputDimension = Vector3.Zero;

            var entitySearchTreeMock = new Mock<EntitySearchTree>();
            entitySearchTreeMock.Setup(
                mock => mock.SearchInArea(
                    inputCenterPosition,
                    inputDimension
                )
            ).ReturnsAsync(
                new List<SearchEntity>()
                {
                    new SearchEntity(
                        expected,
                        Vector3.Zero,
                        new List<string>()
                    )
                }
            );

            // When
            var handler = new SearchInBoundingAreaHandler(
                entitySearchTreeMock.Object
            );
            var actual = await handler.Handle(
                new SearchInBoundingAreaCommand()
                {
                    SearchPositionCenter = inputCenterPosition,
                    SearchDimension = inputDimension
                },
                CancellationToken.None
            );

            // Then
            Assert.Collection(
                actual,
                entityId => Assert.Equal(expected, entityId)
            );
        }
        [Fact]
        public async Task TestShouldEmptyListWhenEntitySearchTreeReturnsNull()
        {
            // Given
            var inputCenterPosition = Vector3.Zero;
            var inputDimension = Vector3.Zero;

            var entitySearchTreeMock = new Mock<EntitySearchTree>();
            entitySearchTreeMock.Setup(
                mock => mock.SearchInArea(
                    inputCenterPosition,
                    inputDimension
                )
            ).ReturnsAsync(
                null as List<SearchEntity>
            );

            // When
            var handler = new SearchInBoundingAreaHandler(
                entitySearchTreeMock.Object
            );
            var actual = await handler.Handle(
                new SearchInBoundingAreaCommand()
                {
                    SearchPositionCenter = inputCenterPosition,
                    SearchDimension = inputDimension
                },
                CancellationToken.None
            );

            // Then
            Assert.Empty(
                actual
            );
        }
    }
}