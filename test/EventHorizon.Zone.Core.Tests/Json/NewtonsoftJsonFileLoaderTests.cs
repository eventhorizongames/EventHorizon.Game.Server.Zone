using System;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Json;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.Json
{
    public class NewtonsoftJsonFileLoaderTests
    {
        [Fact]
        public async Task TestShouldReturnDefaultTypeWhenFileDoesNotExist()
        {
            // Given
            var fakeFile = "this-path-better-not-show-up-some-time-in-the-future/file.json";
            TestDataFile expected = null;

            // When
            var jsonFileLoader = new NewtonsoftJsonFileLoader();
            var actual = await jsonFileLoader.GetFile<TestDataFile>(
                fakeFile
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldReturnDeserializeObjectWhenFileDoesExist()
        {
            // Given
            var realFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Json",
                "TestData",
                "TestFile.json"
            );
            var expected = "test-property-value";

            // When
            var jsonFileLoader = new NewtonsoftJsonFileLoader();
            var actual = await jsonFileLoader.GetFile<TestDataFile>(
                realFile
            );

            // Then
            Assert.Equal(
                expected,
                actual.TestProperty
            );
        }

        public class TestDataFile
        {
            public string TestProperty { get; set; }
        }
    }
}