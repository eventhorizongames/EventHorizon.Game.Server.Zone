namespace EventHorizon.Zone.Core.Map.Tests.Cost
{
    using System.Numerics;
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

    public class RemoveEdgeCostForNodeAtPositionHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateDensityAndTheInverseOfCostWhenHandlingCost()
        {
            // Given
            var position = Vector3.One;
            var node = new MapNode(position);
            var cost = 100;
            var expectedCost = -cost;
            var expectedDense = -1;

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        position
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                node
            );

            // When
            var handler = new RemoveEdgeCostForNodeAtPositionHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RemoveEdgeCostForNodeAtPosition(
                    position,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeTrue();
            mediatorMock.Verify(
                mock => mock.Send(
                    new UpdateDensityAndCostDetailsForNode(
                        node,
                        expectedDense,
                        expectedCost
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
