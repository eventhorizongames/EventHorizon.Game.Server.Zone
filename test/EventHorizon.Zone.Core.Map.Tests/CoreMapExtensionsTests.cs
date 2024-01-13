namespace EventHorizon.Zone.Core.Map.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.Core.Map.Model;
using EventHorizon.Zone.Core.Map.Search;
using EventHorizon.Zone.Core.Map.State;
using EventHorizon.Zone.Core.Model.Map;

using Moq;

using Xunit;

public class CoreMapExtensionsTests
{
    [Fact]
    public void ShouldAddExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();
        var expectedMap = new Mock<IMapGraph>().Object;
        var expectedMapDetails = new Mock<IMapDetails>().Object;
        var expectedMapMesh = new Mock<IMapMesh>().Object;

        // When
        CoreMapExtensions.AddCoreMap(
            serviceCollectionMock
        );

        // Then
        Assert.NotEmpty(
            serviceCollectionMock
        );
        Assert.Collection(
            serviceCollectionMock,
            service =>
            {
                Assert.Equal(
                    typeof(IServerMap),
                    service.ServiceType
                );
                Assert.Equal(
                    typeof(InMemoryServerMap),
                    service.ImplementationInstance.GetType()
                );
                var serverMap = service.ImplementationInstance as InMemoryServerMap;
                serverMap.SetMap(
                    expectedMap
                );
                serverMap.SetMapDetails(
                    expectedMapDetails
                );
                serverMap.SetMapMesh(
                    expectedMapMesh
                );
            },
            service =>
            {
                Assert.Equal(
                    typeof(PathFindingAlgorithm),
                    service.ServiceType
                );
                Assert.Equal(
                    typeof(AStarSearch),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(
                    typeof(IMapGraph),
                    service.ServiceType
                );
                Assert.Equal(
                    expectedMap,
                    service.ImplementationFactory(null)
                );
            },
            service =>
            {
                Assert.Equal(
                    typeof(IMapDetails),
                    service.ServiceType
                );
                Assert.Equal(
                    expectedMapDetails,
                    service.ImplementationFactory(null)
                );
            },
            service =>
            {
                Assert.Equal(
                    typeof(IMapMesh),
                    service.ServiceType
                );
                Assert.Equal(
                    expectedMapMesh,
                    service.ImplementationFactory(null)
                );
            }
        );
    }
}
