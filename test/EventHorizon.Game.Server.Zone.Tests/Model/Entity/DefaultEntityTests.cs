using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Model.Entity
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

        [Fact]
        public void TestGetProperty_ShouldReturnPropertyFromData()
        {
            //Given
            var expected = "Some Data Property";

            //When
            var defaultEntity = new DefaultEntity()
            {
                Data = new Dictionary<string, object>()
            };
            defaultEntity.Data["someProperty"] = expected;
            var actual = defaultEntity.GetProperty<string>("someProperty");

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetData_ShouldReturnObjectStateFromData()
        {
            //Given
            var expected = "Some Data Property";

            //When
            var defaultEntity = new DefaultEntity()
            {
                Data = new Dictionary<string, object>()
                {
                    {
                        "SomeData",
                        expected
                    }
                }
            };
            var model = defaultEntity.GetProperty<string>("SomeData");
            var actual = model;

            //Then
            Assert.Equal(expected, actual);
        }
    }
}