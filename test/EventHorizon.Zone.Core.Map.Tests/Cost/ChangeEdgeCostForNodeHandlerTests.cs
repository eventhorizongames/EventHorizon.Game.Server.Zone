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

    public class ChangeEdgeCostForNodeHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateDentityAndCostDetailsForNodeWhenRequestIsHandled()
        {
            // Given
            var node = new MapNode();
            var cost = 100;

            var meditorMock = new Mock<IMediator>();

            // When
            var handler = new ChangeEdgeCostForNodeHandler(
                meditorMock.Object
            );
            var actual = await handler.Handle(
                new ChangeEdgeCostForNode(
                    node,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeTrue();
            meditorMock.Verify(
                mock => mock.Send(
                    new UpdateDensityAndCostDetailsForNode(
                        node,
                        1,
                        cost
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
