using System;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Json;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.Json
{
    public class NewtonsoftJsonFileSaverTests
    {
        string directory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Json",
            "TestData-Saver"
        );
        string fileName = "TestFile.json";

        public NewtonsoftJsonFileSaverTests()
        {
            // Remove existing test file
            var fullFileName = Path.Combine(
                directory,
                fileName
            );
            File.Delete(
                fullFileName
            );
        }

        [Fact]
        public async Task TestShouldSaveSerializedRepresentationWhenObjectIsPassedToSpecifiedDirectoryAndFileName()
        {
            // Given 
            var fullFileName = Path.Combine(
                directory,
                fileName
            );
            var testData = new TestDataFile
            {
                TestProperty = "test-property-value"
            };

            // Verify File is not there
            Assert.False(
                File.Exists(
                    fullFileName
                )
            );

            // When
            var jsonFileSaver = new NewtonsoftJsonFileSaver();

            await jsonFileSaver.SaveToFile(
                directory,
                fileName,
                testData
            );

            // Then
            Assert.True(
                File.Exists(
                    fullFileName
                )
            );
        }

        public class TestDataFile
        {
            public string TestProperty { get; set; }
        }
    }
}