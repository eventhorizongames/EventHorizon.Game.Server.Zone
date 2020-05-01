namespace EventHorizon.Zone.Core.Map.Tests.Find
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Map.Find;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class GetMapEdgesOfNodeHandlerTests
    {
        [Fact]
        public async Task ShouldReturnEdgesForNodeWhenRequestIsHandled()
        {
            // Given
            var fromIndex = 123;
            var nodeIndex = 102;
            var edges = new List<MapEdge>
            {
                new MapEdge(
                    fromIndex,
                    nodeIndex
                )
            };

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetEdgesOfNode(
                    nodeIndex
                )
            ).Returns(
                edges
            );

            // When
            var handler = new GetMapEdgesOfNodeHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapEdgesOfNodeEvent(
                    nodeIndex
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .NotBeEmpty();
            actual.Should()
                .BeEquivalentTo(edges);
        }
    }
}