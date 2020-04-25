namespace EventHorizon.Game.Server.Zone.Tests.Model.Entity
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Xunit;

    public class DefaultEntityTests
    {
        [Fact]
        public void Test_ShouldHaveExpectedValues()
        {
            //Given
            var expectedId = 0L;
            var expectedType = EntityType.OTHER;
            var expectedTransform = default(
                TransformState
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
                expectedTransform,
                actual.Transform
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
                RawData = new ConcurrentDictionary<string, object>()
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
            var rawData = new ConcurrentDictionary<string, object>();
            rawData["SomeData"] = expected;
            var defaultEntity = new DefaultEntity()
            {
                RawData = rawData,
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