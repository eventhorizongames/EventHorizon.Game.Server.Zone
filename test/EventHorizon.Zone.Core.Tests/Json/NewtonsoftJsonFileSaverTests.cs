namespace EventHorizon.Zone.Core.Tests.Json;

using System;
using System.IO;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Json;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using Moq;

using Xunit;

public class NewtonsoftJsonFileSaverTests
{
    [Fact]
    public async Task TestShouldSaveSerializedRepresentationWhenObjectIsPassedToSpecifiedDirectoryAndFileName()
    {
        // Given 
        var directory = "directory";
        var fileName = "file-name";
        var testData = new TestDataFile
        {
            TestProperty = "test-property-value"
        };
        var expectedFileFullName = Path.Combine(
            directory,
            fileName
        );
        var expectedText = @"{""TestProperty"":""test-property-value""}";

        var directoryResolverMock = new Mock<DirectoryResolver>();
        var fileResolverMock = new Mock<FileResolver>();

        directoryResolverMock.Setup(
            mock => mock.CreateDirectory(
                directory
            )
        ).Returns(
            true
        );

        // When
        var jsonFileSaver = new NewtonsoftJsonFileSaver(
            directoryResolverMock.Object,
            fileResolverMock.Object
        );

        await jsonFileSaver.SaveToFile(
            directory,
            fileName,
            testData
        );

        // Then
        fileResolverMock.Verify(
            mock => mock.WriteAllText(
                expectedFileFullName,
                expectedText
            )
        );
    }

    public class TestDataFile
    {
        public string TestProperty { get; set; }
    }
}
