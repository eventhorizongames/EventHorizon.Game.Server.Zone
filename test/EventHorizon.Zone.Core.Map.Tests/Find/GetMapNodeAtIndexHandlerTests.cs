namespace EventHorizon.Zone.Core.Map.Tests.Find;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Map.Find;
using EventHorizon.Zone.Core.Model.Map;

using FluentAssertions;

using Moq;

using Xunit;

public class GetMapNodeAtIndexHandlerTests
{
    [Fact]
    public async Task ShouldReturnNodeForRequestedNodeIndexWhenRequestIsHandled()
    {
        // Given
        var nodeIndex = 101;
        var expected = new MapNode(
            nodeIndex
        );

        var mapGraphMock = new Mock<IMapGraph>();

        mapGraphMock.Setup(
            mock => mock.GetNode(
                nodeIndex
            )
        ).Returns(
            expected
        );

        // When
        var handler = new GetMapNodeAtIndexHandler(
            mapGraphMock.Object
        );
        var actual = await handler.Handle(
            new GetMapNodeAtIndexEvent(
                nodeIndex
            ),
            CancellationToken.None
        );

        // Then
        actual.Should()
            .Be(expected);
    }
}
