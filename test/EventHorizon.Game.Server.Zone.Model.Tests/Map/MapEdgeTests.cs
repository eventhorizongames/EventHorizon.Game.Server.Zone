using EventHorizon.Game.Server.Zone.Model.Map;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Model.Tests.Map
{
    public class MapEdgeTests
    {
        [Fact]
        public void Test_ShouldHaveExpectedValuesWhenCreated()
        {
            //Given
            var expectedFromIndex = 1;
            var expectedToIndex = 43;
            var expectedCost = 0;

            //When
            var actual = new MapEdge(expectedFromIndex, expectedToIndex);

            //Then
            Assert.Equal(expectedFromIndex, actual.FromIndex);
            Assert.Equal(expectedToIndex, actual.ToIndex);
            Assert.Equal(expectedCost, actual.Cost);
        }

        [Fact]
        public void Test_WhenNullShouldReturnExpectedNullValues()
        {
            //Given
            var expectedFromIndex = -1;
            var expectedToIndex = -1;
            var expectedCost = 0;

            //When
            var actual = MapEdge.NULL;

            //Then
            Assert.Equal(expectedFromIndex, actual.FromIndex);
            Assert.Equal(expectedToIndex, actual.ToIndex);
            Assert.Equal(expectedCost, actual.Cost);
        }
    }
}