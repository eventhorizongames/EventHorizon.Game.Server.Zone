using Xunit;
using System.Threading.Tasks;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Map;

namespace EventHorizon.Game.Server.Zone.Tests.Model.Map
{
    public class MapNodeTests
    {
        [Fact]
        public void TestEquals_WhenInputNullShouldReturnFalse()
        {
            // Given
            string input = null;

            // When
            var mapNode = new MapNode(Vector3.Zero);

            var actual = mapNode.Equals(input);

            // Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsNotMapNodeTypeShouldReturnFalse()
        {
            // Given
            string input = "not-map-node";

            // When
            var mapNode = new MapNode(Vector3.Zero);

            var actual = mapNode.Equals(input);

            // Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsMapNodeNotSameIdShouldReturnFalse()
        {
            // Given
            var inputId = 123;
            MapNode input = new MapNode(Vector3.Zero)
            {
                Index = inputId
            };

            // When
            var mapNode = new MapNode(Vector3.Zero)
            {
                Index = 321
            };

            var actual = mapNode.Equals(input);

            // Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsMapNodeSameIdShouldReturnTrue()
        {
            // Given
            var inputId = 123;
            MapNode input = new MapNode(Vector3.Zero)
            {
                Index = inputId
            };

            // When
            var mapNode = new MapNode(Vector3.Zero)
            {
                Index = inputId
            };

            var actual = mapNode.Equals(input);

            // Then
            Assert.True(actual);
        }

        [Fact]
        public void TestGetHashCode_ShouldReturnTheHashCodeOfTheIndex()
        {
            // Given
            var inputIndex = 123;
            var expected = inputIndex.GetHashCode();

            // When
            var entityAction = new MapNode(Vector3.Zero)
            {
                Index = inputIndex
            };

            var actual = entityAction.GetHashCode();

            // Then
            Assert.Equal(expected, actual);
        }
    }
}