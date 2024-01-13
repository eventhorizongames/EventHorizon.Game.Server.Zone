namespace EventHorizon.Zone.Core.Map.Tests.Generated
{
    using System.Linq;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Generate;
    using EventHorizon.Zone.Core.Map.Generate;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using Xunit;

    public class GenerateMapFromDetailsHandlerTests
    {
        [Fact]
        public async Task ShouldReturnMapGeneratedFromDetailsOfRequest()
        {
            // Given
            var dimensions = 10;
            var tileDimensions = 1;

            var expectedNumberOfNodes = 100;
            var expectedNumberOfEdges = 360;

            var expectedFirstNodePosition = new Vector3(-5, 0, -5);
            var expectedLastNodePosition = new Vector3(4, 0, 4);

            var expectedEdge1 = new MapEdge(49, 48);
            var expectedEdge2 = new MapEdge(49, 39);

            // When
            var handler = new GenerateMapFromDetailsHandler();
            var actual = await handler.Handle(
                new GenerateMapFromDetails(
                    new MapDetailsMock { Dimensions = dimensions, TileDimensions = tileDimensions, }
                ),
                CancellationToken.None
            );

            // Then
            actual.NumberOfNodes.Should().Be(expectedNumberOfNodes);
            actual.EdgeList.Count.Should().Be(expectedNumberOfEdges);

            var firstNode = actual.NodeList.First();
            var lastNode = actual.NodeList.Last();
            firstNode.Position.Should().Be(expectedFirstNodePosition, "it is the first node");
            lastNode.Position.Should().Be(expectedLastNodePosition, "it is the last node");

            actual.EdgeList.Should().Contain(expectedEdge1);
            actual.EdgeList.Should().Contain(expectedEdge2);
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task ShouldReturnLargeMapGeneratedFromDetailsOfRequest()
        {
            // Given
            var dimensions = 124;
            var tileDimensions = 1;

            var expectedNumberOfNodes = 15376;
            var expectedNumberOfEdges = 61008;

            var expectedFirstNodePosition = new Vector3(-62, 0, -62);
            var expected100thNodePosition = new Vector3(-62, 0, 38);
            var expected1000thNodePosition = new Vector3(-54, 0, -54);
            var expected10000thNodePosition = new Vector3(18, 0, 18);
            var expectedLastNodePosition = new Vector3(61, 0, 61);

            var expectedFirstEdge = new MapEdge(8446, 8570);
            var expected100thEdge = new MapEdge(8471, 8595);
            var expected1000thEdge = new MapEdge(8697, 8821);
            var expected10000thEdge = new MapEdge(9975, 9976);
            var expectedLastEdge = new MapEdge(8453, 8329);

            // When
            var handler = new GenerateMapFromDetailsHandler();
            var actual = await handler.Handle(
                new GenerateMapFromDetails(
                    new MapDetailsMock { Dimensions = dimensions, TileDimensions = tileDimensions, }
                ),
                CancellationToken.None
            );

            // Then
            actual.NumberOfNodes.Should().Be(expectedNumberOfNodes);
            actual.EdgeList.Count.Should().Be(expectedNumberOfEdges);

            actual
                .NodeList.First()
                .Position.Should()
                .Be(expectedFirstNodePosition, "it is the first node");
            actual
                .NodeList.ElementAt(100)
                .Position.Should()
                .Be(expected100thNodePosition, "it is the 100th node");
            actual
                .NodeList.ElementAt(1000)
                .Position.Should()
                .Be(expected1000thNodePosition, "it is the 1000th node");
            actual
                .NodeList.ElementAt(10000)
                .Position.Should()
                .Be(expected10000thNodePosition, "it is the 10000th node");
            actual
                .NodeList.Last()
                .Position.Should()
                .Be(expectedLastNodePosition, "it is the last node");

            actual.EdgeList.Should().Contain(expectedFirstEdge, "it is the first edge");
            actual.EdgeList.Should().Contain(expected100thEdge, "it is the 100th edge");
            actual.EdgeList.Should().Contain(expected1000thEdge, "it is the 1000th edge");
            actual.EdgeList.Should().Contain(expected10000thEdge, "it is the 10000th edge");
            actual.EdgeList.Should().Contain(expectedLastEdge, "it is the last edge");
        }
    }

    public struct MapDetailsMock : IMapDetails
    {
        public int Dimensions { get; set; }

        public int TileDimensions { get; set; }
    }
}
