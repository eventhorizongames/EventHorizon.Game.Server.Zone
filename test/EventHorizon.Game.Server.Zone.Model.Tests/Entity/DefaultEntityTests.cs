using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Model.Tests.Entity
{
    public class DefaultEntityTests
    {
        [Fact]
        public void Test_ShouldHaveExpectedValues()
        {
            //Given
            var expectedId = 0;
            var expectedType = EntityType.OTHER;
            var expectedPosition = default(PositionState);
            IList<string> expectedTagList = null;
            dynamic expectedData = null;

            //When
            var actual = new DefaultEntity();

            //Then
            Assert.False(actual.IsFound());
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedType, actual.Type);
            Assert.Equal(expectedPosition, actual.Position);
            Assert.Equal(expectedTagList, actual.TagList);
            Assert.Equal(expectedData, actual.Data);
        }

        [Fact]
        public void Test_ShouldMatchDefault()
        {
            //Given
            var expected = default(DefaultEntity);

            //When
            var actual = new DefaultEntity();

            //Then
            Assert.False(actual.IsFound());
            Assert.Equal(expected, actual);
        }
    }
}