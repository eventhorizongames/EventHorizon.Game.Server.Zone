namespace EventHorizon.Zone.Core.Map.Tests.Find
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Map.Find;
    using EventHorizon.Zone.Core.Model.Map;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class GetMapNodeCountHandlerTests
    {
        [Fact]
        public async Task ShoulReturnNumberOfNodesFromMapGraphWhenRequestIsHandled()
        {
            // Given
            var expected = 101;

            var mapGraphMock = new Mock<IMapGraph>();

            mapGraphMock.Setup(
                mock => mock.NumberOfNodes
            ).Returns(
                expected
            );

            // When
            var handler = new GetMapNodeCountHandler(
                mapGraphMock.Object
            );
            var actual = await handler.Handle(
                new GetMapNodeCountEvent(),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .Be(expected);
        }
    }
}
