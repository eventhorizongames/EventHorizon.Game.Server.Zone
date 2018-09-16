using Xunit;
using Moq;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.Search.Handler;
using EventHorizon.Game.Server.Zone.Map.Create;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Load.Map.Model;

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
            var expectedDimensions = new Vector3(inputDimensions * inputTileDimensions);

            var zoneMapFactoryMock = new Mock<IZoneMapFactory>();
            var entitySearchTreeMock = new Mock<IEntitySearchTree>();

            zoneMapFactoryMock.Setup(a => a.Map).Returns(new ZoneMap
            {
                Dimensions = inputDimensions,
                TileDimensions = inputTileDimensions
            });

            // When
            var entitySearchMapCreatedHandler = new EntitySearchMapCreatedHandler(
                zoneMapFactoryMock.Object,
                entitySearchTreeMock.Object
            );

            await entitySearchMapCreatedHandler.Handle(new MapCreatedEvent(), CancellationToken.None);

            // Then
            entitySearchTreeMock.Verify(a => a.UpdateDimensions(expectedDimensions));
        }
    }
}