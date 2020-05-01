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

    public class GetMapNodesInDimensionsCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnListOfClosetsNodesInDimensionWhenRequestIsHandled()
        {
            // Given
            var position = new Vector3(2, 3, 3);
            var dimensions = new Vector3(2, 23, 4);
            var expected = new List<MapNode>
            {
                new MapNode()
            };

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.GetClosestNodesInDimension(
                    position,
                    dimensions
                )
            ).Returns(
                expected
            );

            // When
            var handler = new GetMapNodesInDimensionsCommandHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNodesInDimensionsCommand(
                    position,
                    dimensions
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeEquivalentTo(expected);
        }
    }
}