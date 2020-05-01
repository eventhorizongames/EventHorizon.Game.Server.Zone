namespace EventHorizon.Zone.Core.Map.Tests.Cost
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Map.Cost;
    using EventHorizon.Zone.Core.Model.Map;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using Xunit;

    public class UpdateDensityAndCostDetailsForNodeHandlerTests
    {
        [Fact]
        public async Task ShouldChangeEdgeCostForPassedInNodeWhenEdgesAreFoundForNode()
        {
            // Given
            var fromIndex = 102;
            var nodeIndex = 101;
            var node = new MapNode(
                nodeIndex
            );
            var dense = 1;
            var cost = 100;
            var edge = new MapEdge(
                fromIndex,
                nodeIndex
            );
            var expected = edge;
            expected.Cost += cost;

            var mediatorMock = new Mock<IMediator>();
            var mapGraphMock = new Mock<IMapGraph>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapEdgesOfNodeEvent(
                        nodeIndex
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<MapEdge>
                {
                    edge
                }
            );

            // When
            var handler = new UpdateDensityAndCostDetailsForNodeHandler(
                mediatorMock.Object,
                mapGraphMock.Object
            );
            await handler.Handle(
                new UpdateDensityAndCostDetailsForNode(
                    node,
                    dense,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            mapGraphMock.Verify(
                mock => mock.RemoveEdge(
                    edge
                )
            );
            mapGraphMock.Verify(
                mock => mock.AddEdge(
                    expected
                )
            );
        }

        [Fact]
        public async Task ShouldUpdateNodeWithPassedInDenseParameter()
        {
            // Given
            var fromIndex = 102;
            var nodeIndex = 101;
            var node = new MapNode(
                nodeIndex
            );
            var dense = 1023;
            var cost = 100;
            var edge = new MapEdge(
                fromIndex,
                nodeIndex
            );

            var mediatorMock = new Mock<IMediator>();
            var mapGraphMock = new Mock<IMapGraph>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapEdgesOfNodeEvent(
                        nodeIndex
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<MapEdge>
                {
                    edge
                }
            );

            // When
            var handler = new UpdateDensityAndCostDetailsForNodeHandler(
                mediatorMock.Object,
                mapGraphMock.Object
            );
            await handler.Handle(
                new UpdateDensityAndCostDetailsForNode(
                    node,
                    dense,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            node.Info["dense"]
                .Should().Be(dense);
        }

        [Fact]
        public async Task ShouldAddDenseToNodeDenseInfoValueWhenPassedInDenseParameter()
        {
            // Given
            var expected = 2023;
            var dense = 1023;
            var fromIndex = 102;
            var nodeIndex = 101;
            var node = new MapNode(
                nodeIndex
            );
            node.Info["dense"] = 1000;
            var cost = 100;
            var edge = new MapEdge(
                fromIndex,
                nodeIndex
            );

            var mediatorMock = new Mock<IMediator>();
            var mapGraphMock = new Mock<IMapGraph>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapEdgesOfNodeEvent(
                        nodeIndex
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<MapEdge>
                {
                    edge
                }
            );

            // When
            var handler = new UpdateDensityAndCostDetailsForNodeHandler(
                mediatorMock.Object,
                mapGraphMock.Object
            );
            await handler.Handle(
                new UpdateDensityAndCostDetailsForNode(
                    node,
                    dense,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            node.Info["dense"]
                .Should().Be(expected);
        }
    }
}
