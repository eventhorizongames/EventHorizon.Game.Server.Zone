namespace EventHorizon.Zone.Core.Map.Tests.Find
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Path;
    using EventHorizon.Zone.Core.Map.Find;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;
    using Moq;
    using Xunit;

    public class FindPathHandlerTests
    {
        [Fact]
        public async Task ShouldSearchPathFindingAlgorithmWithFromAndTwoPositionNodes()
        {
            // Given
            var from = new Vector3(3, 3, 3);
            var to = new Vector3(1, 1, 1);
            var fromMapNode = new MapNode(
                from
            );
            var toMapNode = new MapNode(
                to
            );

            var mediatorMock = new Mock<IMediator>();
            var mapGraphMock = new Mock<IMapGraph>();
            var pathFindingAlgorithmMock = new Mock<PathFindingAlgorithm>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNotDenseNodeAtPosition(
                        from
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fromMapNode
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetMapNotDenseNodeAtPosition(
                        to
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                toMapNode
            );

            // When
            var handler = new FindPathHandler(
                mediatorMock.Object,
                mapGraphMock.Object,
                pathFindingAlgorithmMock.Object
            );
            var actual = await handler.Handle(
                new FindPathEvent(
                    from,
                    to
                ),
                CancellationToken.None
            );

            // Then
            pathFindingAlgorithmMock.Verify(
                mock => mock.Search(
                    mapGraphMock.Object,
                    fromMapNode,
                    toMapNode
                )
            );
        }
    }
}