using Xunit;
using Moq;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Search.Handler;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.Core.Events.Map.Create;

namespace EventHorizon.Game.Server.Zone.Tests.Entity.Search.Handler
{
    public class EntitySearchMapCreatedHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallUpdateDimensionsOnSearchTree()
        {
            // Given
            var inputDimensions = 4;
            var inputTileDimensions = 5;
            var expectedDimensions = new Vector3(
                inputDimensions * inputTileDimensions
            );

            var mapDetailsMock = new Mock<IMapDetails>();
            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

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