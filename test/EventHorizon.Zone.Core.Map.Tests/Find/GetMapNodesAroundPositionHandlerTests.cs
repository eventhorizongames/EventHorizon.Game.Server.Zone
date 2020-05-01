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

    public class GetMapNodesAroundPositionHandlerTests
    {
        [Fact]
        public async Task ShouldReturnListOfClosetsNodesWhenRequestIsHandled()
        {
            // Given
            var position = new Vector3(2, 3, 3);
            var distance = 123;
            var expected = new List<MapNode>
            {
                new MapNode()
            };

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetClosestNodes(
                    position,
                    distance
                )
            ).Returns(
                expected
            );

            // When
            var handler = new GetMapNodesAroundPositionHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNodesAroundPositionEvent(
                    position,
                    distance
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeEquivalentTo(expected);
        }
    }
}