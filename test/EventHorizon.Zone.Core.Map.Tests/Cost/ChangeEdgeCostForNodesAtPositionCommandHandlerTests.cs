namespace EventHorizon.Zone.Core.Map.Tests.Cost
{
    using System.Collections.Generic;
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

    public class ChangeEdgeCostForNodesAtPositionCommandHandlerTests
    {
        [Fact]
        public async Task ShouldLookupAndSendChangeEdgeCostForFoundNodesWhenHandlingReqeust()
        {
            // Given
            var position = Vector3.One;
            var boundingBox = new Vector3(3, 3, 3);
            var cost = 1;
            var node = new MapNode(
                position
            );

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNodesInDimensionsCommand(
                        position,
                        boundingBox
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<MapNode>
                {
                    node
                }
            );

            // When
            var handler = new ChangeEdgeCostForNodesAtPositionCommandHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new ChangeEdgeCostForNodesAtPositionCommand(
                    position,
                    boundingBox,
                    cost
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeTrue();
            mediatorMock.Verify(
                mock => mock.Send(
                    new ChangeEdgeCostForNode(
                        node,
                        cost
                    ),
                    CancellationToken.None
                )
            );

        }
    }
}
