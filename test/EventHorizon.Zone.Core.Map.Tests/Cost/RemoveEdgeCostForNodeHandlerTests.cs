namespace EventHorizon.Zone.Core.Map.Tests.Cost
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Map.Cost;
    using EventHorizon.Zone.Core.Model.Map;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class RemoveEdgeCostForNodeHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateDensityAndTheInverseOfCostWhenHandlingCost()
        {
            // Given
            var node = new MapNode(2);
            var cost = 100;
            var expectedCost = -cost;
            var expectedDense = -1;

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RemoveEdgeCostForNodeHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RemoveEdgeCostForNode(
                    node,
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
