using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Model.Tests.Entity
{
    public class SearchEntityTests
    {
        [Fact]
        public void TestEquals_WhenInputIsNullShouldReturnFalse()
        {
            //Given
            string input = null;

            //When
            var searchEntity = new SearchEntity();

            var actual = searchEntity.Equals(input);

            //Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsNotSearchEntityTypeShouldReturnFalse()
        {
            // Given
            string input = "not-search-entity";

            // When
            var searchEntity = new SearchEntity(123, Vector3.Zero, null);

            var actual = searchEntity.Equals(input);

            // Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsSearchEntityNotSameIdShouldReturnFalse()
        {
            // Given
            var inputId = 123;
            var input = new SearchEntity(inputId, Vector3.Zero, null);

            // When
            var actualId = 321;
            var searchEntity = new SearchEntity(actualId, Vector3.Zero, null);

            var actual = searchEntity.Equals(input);

            // Then
            Assert.False(actual);
        }

        [Fact]
        public void TestEquals_WhenInputIsSearchEntitySameIdShouldReturnTrue()
        {
            // Given
            var inputId = 123;
            SearchEntity input = new SearchEntity(inputId, Vector3.Zero, null);

            // When
            var searchEntity = new SearchEntity(inputId, Vector3.Zero, null);

            var actual = searchEntity.Equals(input);

            // Then
            Assert.True(actual);
        }

        [Fact]
        public void TestGetHashCode_WhenEntityIdIsNotNullShouldReturnHashCodeOfEntityId()
        {
            // Given
            var inputId = 123;
            var expected = inputId.GetHashCode();

            // When
            var searchEntity = new SearchEntity(inputId, Vector3.Zero, null);

            var actual = searchEntity.GetHashCode();

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestProperties_ShouldBeAbleToReadPropertiesPassedInThroughConstructor()
        {
            //Given
            var expectedEntityId = 123;
            var expectedPosition = new Vector3(2);
            var expectedTagList = new List<string>()
            {
                "tag1",
                "tag2"
            };
            //When
            var actual = new SearchEntity(expectedEntityId, expectedPosition, expectedTagList);

            //Then
            Assert.Equal(expectedEntityId, actual.EntityId);
            Assert.Equal(expectedPosition, actual.Position);
            Assert.Collection(actual.TagList,
                a => Assert.Equal(a, expectedTagList[0]),
                a => Assert.Equal(a, expectedTagList[1])
            );
        }
    }
}