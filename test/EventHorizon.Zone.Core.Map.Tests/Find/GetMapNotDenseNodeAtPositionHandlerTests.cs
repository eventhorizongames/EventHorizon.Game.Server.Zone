namespace EventHorizon.Zone.Core.Map.Tests.Find
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Map.Find;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class GetMapNotDenseNodeAtPositionHandlerTests
    {
        [Fact]
        public async Task ShouldReturnClosestNotDenseNodeWhenRequestIsHandled()
        {
            // Given
            var position = new Vector3(2, 3, 3);
            var dimensions = new Vector3(2, 23, 4);
            var expected = new MapNode(
                position
            );

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetClosestNode(
                    position
                )
            ).Returns(
                expected
            );

            // When
            var handler = new GetMapNotDenseNodeAtPositionHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNotDenseNodeAtPosition(
                    position
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnClosestNotDenseNodeWhenGetClosestNodeIsDense()
        {
            // Given
            var position = new Vector3(2, 2, 2);
            var fistNodePosition = new Vector3(2, 3, 3);
            var firstNode = new MapNode(
                fistNodePosition
            );
            firstNode.Info["dense"] = 1;

            var distanceToCheck = 1;
            var expectedNodePosition = new Vector3(3, 3, 3);
            var expected = new MapNode(
                expectedNodePosition
            );

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetClosestNode(
                    position
                )
            ).Returns(
                firstNode
            );

            mapGraphMock.Setup(
                mock => mock.GetClosestNodes(
                    position,
                    distanceToCheck
                )
            ).Returns(
                new List<MapNode>
                {
                    expected
                }
            );

            // When
            var handler = new GetMapNotDenseNodeAtPositionHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNotDenseNodeAtPosition(
                    position
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnNotFoundNodeWhenNotAbleToFindNonDenseNode()
        {
            // Given
            var deltaToCheck = 10;
            var position = new Vector3(2, 2, 2);
            var fistNodePosition = new Vector3(2, 3, 3);
            var firstNode = new MapNode(
                fistNodePosition
            );
            firstNode.Info["dense"] = 1;

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetClosestNode(
                    position
                )
            ).Returns(
                firstNode
            );

            for (int i = 0; i < deltaToCheck + 2; i++)
            {
                mapGraphMock.Setup(
                    mock => mock.GetClosestNodes(
                        position,
                        i
                    )
                ).Returns(
                    new List<MapNode>()
                );
            }

            // When
            var handler = new GetMapNotDenseNodeAtPositionHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNotDenseNodeAtPosition(
                    position
                ),
                CancellationToken.None
            );

            // Then
            actual.IsFound().Should().BeFalse();
        }
    }
}