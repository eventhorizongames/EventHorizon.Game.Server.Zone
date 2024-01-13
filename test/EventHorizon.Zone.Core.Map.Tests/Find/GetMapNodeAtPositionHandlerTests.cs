namespace EventHorizon.Zone.Core.Map.Tests.Find;

using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Map.Find;
using EventHorizon.Zone.Core.Model.Map;

using FluentAssertions;

using Moq;

using Xunit;

public class GetMapNodeAtPositionHandlerTests
{
    [Fact]
    public async Task ShouldReturnClosestNodeForRequestedPositionWhenRequestIsHandled()
    {
        // Given
        var position = new Vector3(3, 3, 3);
        var expected = new MapNode(
            position
        );

        var mapGraphMock = new Mock<IMapGraph>();

        mapGraphMock.Setup(
            mock => mock.GetClosestNode(
                position
            )
        ).Returns(
            expected
        );

        // When
        var handler = new GetMapNodeAtPositionHandler(
            mapGraphMock.Object
        );
        var actual = await handler.Handle(
            new GetMapNodeAtPositionEvent(
                position
            ),
            CancellationToken.None
        );

        // Then
        actual.Should()
            .Be(expected);
    }
}
