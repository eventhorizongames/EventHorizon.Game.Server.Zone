namespace EventHorizon.Zone.Core.Entity.Tests.Search
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Entity.Search;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Model.Map;
    using Moq;
    using Xunit;

    public class EntitySearchMapCreatedHandlerTests
    {
        [Fact]
        public async Task TestShouldCallUpdateDimensionsOnSearchTree()
        {
            // Given
            var inputDimensions = 4;
            var inputTileDimensions = 5;
            var expectedDimensions = new Vector3(
                inputDimensions * inputTileDimensions
            );

            var mapDetailsMock = new Mock<IMapDetails>();
            var entitySearchTreeMock = new Mock<EntitySearchTree>();

            mapDetailsMock.Setup(
                mock => mock.Dimensions
            ).Returns(
                inputDimensions
            );
            mapDetailsMock.Setup(
                mock => mock.TileDimensions
            ).Returns(
                inputTileDimensions
            );

            // When
            var entitySearchMapCreatedHandler = new EntitySearchMapCreatedHandler(
                mapDetailsMock.Object,
                entitySearchTreeMock.Object
            );

            await entitySearchMapCreatedHandler.Handle(
                new MapCreatedEvent(),
                CancellationToken.None
            );

            // Then
            entitySearchTreeMock.Verify(
                mock => mock.UpdateDimensions(
                    expectedDimensions
                )
            );
        }
    }
}