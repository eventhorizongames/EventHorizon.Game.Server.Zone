namespace EventHorizon.Zone.Core.Map.Tests.State
{
    using EventHorizon.Zone.Core.Map.State;
    using EventHorizon.Zone.Core.Model.Map;

    using Moq;

    using Xunit;

    public class InMemoryServerMapTests
    {
        [Fact]
        public void TestSetMap_ShouldSetMapGraphInToServerMap()
        {
            // Given
            var expected = new Mock<IMapGraph>();

            // When
            var serverMap = new InMemoryServerMap();

            serverMap.SetMap(
                expected.Object
            );

            var actual = serverMap.Map();

            // Then
            Assert.Equal(
                expected.Object,
                actual
            );
        }
        [Fact]
        public void TestSetMap_ShouldSetMapDetailsInToServerMap()
        {
            // Given
            var expected = new Mock<IMapDetails>();

            // When
            var serverMap = new InMemoryServerMap();

            serverMap.SetMapDetails(
                expected.Object
            );

            var actual = serverMap.MapDetails();

            // Then
            Assert.Equal(
                expected.Object,
                actual
            );
        }
        [Fact]
        public void TestSetMap_ShouldSetMapMeshInToServerMap()
        {
            // Given
            var expected = new Mock<IMapMesh>();

            // When
            var serverMap = new InMemoryServerMap();

            serverMap.SetMapMesh(
                expected.Object
            );

            var actual = serverMap.MapMesh();

            // Then
            Assert.Equal(
                expected.Object,
                actual
            );
        }
    }
}
