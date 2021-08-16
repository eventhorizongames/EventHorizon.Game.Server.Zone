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

    public class ChangeEdgeCostForNodeAtPositionHandlerTests
    {
        [Fact]
        public async Task ShouldUpdateDentityAndCostDetailsForNodeWhenRequestIsHandled()
        {
            // Given
            var position = Vector3.One;
            var nodeAtPosition = new MapNode(
                position
            );
            var cost = 100;

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodeAtPositionEvent(
                        position
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                nodeAtPosition
            );

            // When
            var handler = new ChangeEdgeCostForNodeAtPositionHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new ChangeEdgeCostForNodeAtPosition(
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
                        nodeAtPosition,
                        1,
                        cost
                    ),
                    CancellationToken.None
                )
            );

        }
    }
}
