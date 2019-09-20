using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
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
            var expectedPosition = default(
                PositionState
            );
            Dictionary<string, object> expectedData = new Dictionary<string, object>();

            //When
            var actual = new DefaultEntity();

            //Then
            Assert.False(
                actual.IsFound()
            );
            Assert.Equal(
                expectedId,
                actual.Id
            );
            Assert.Equal(
                expectedType,
                actual.Type
            );
            Assert.Equal(
                expectedPosition,
                actual.Position
            );
            Assert.Null(
                actual.TagList
            );
            Assert.Equal(
                expectedData, actual.Data
            );
        }

        [Fact]
        public void Test_ShouldMatchDefault()
        {
            //Given
            var expected = new DefaultEntity();

            //When
            var actual = new DefaultEntity();

            //Then
            Assert.False(
                actual.IsFound()
            );
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestGetProperty_ShouldReturnPropertyFromData()
        {
            //Given
            var expected = "Some Data Property";

            //When
            var defaultEntity = new DefaultEntity()
            {
                RawData = new Dictionary<string, object>()
            };
            defaultEntity.Data["someProperty"] = expected;
            var actual = defaultEntity.GetProperty<string>(
                "someProperty"
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestGetData_ShouldReturnObjectStateFromData()
        {
            //Given
            var expected = "Some Data Property";

            //When
            var defaultEntity = new DefaultEntity()
            {
                RawData = new Dictionary<string, object>()
                {
                    {
                        "SomeData",
                        expected
                    }
                }
            };
            defaultEntity.PopulateData<string>(
                "SomeData"
            );
            var actual = defaultEntity.GetProperty<string>(
                "SomeData"
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}