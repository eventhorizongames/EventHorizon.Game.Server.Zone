namespace EventHorizon.Zone.Core.Tests.FileService;

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.FileService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using FluentAssertions;

using Moq;

using Xunit;


public class WriteResourceToFileHandlerTests
{
    [Fact]
    public async Task ShouldReturnSuccessWhenContentIsWrittenToFile()
    {
        // Given
        var expectedFileContent = "embedded-resource-file-content";

        var assembly = Assembly.GetExecutingAssembly();
        var resourceRoot = "EventHorizon.Zone.Core.Tests";
        var resourcePath = "FileService.App_Data";
        var resourceFile = "EmbeddedResource.txt";
        var saveFileFullName = Path.Combine(
            "the",
            "path",
            "to",
            "EmbeddedResource.txt"
        );

        var fileResolverMock = new Mock<FileResolver>();
        var directoryResolverMock = new Mock<DirectoryResolver>();

        fileResolverMock.Setup(
            mock => mock.DoesFileExist(
                saveFileFullName
            )
        ).Returns(
            false
        );

        // When
        var handler = new WriteResourceToFileHandler(
            fileResolverMock.Object,
            directoryResolverMock.Object
        );
        var actual = await handler.Handle(
            new WriteResourceToFile(
                assembly,
                resourceRoot,
                resourcePath,
                resourceFile,
                saveFileFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        fileResolverMock.Verify(
            mock => mock.WriteAllText(
                saveFileFullName,
                expectedFileContent
            )
        );
    }

    [Fact]
    public async Task ShouldReturnFileAlreadyExistsWheSaveFileFullNameExists()
    {
        // Given
        var expected = "file_already_exists";

        var assembly = Assembly.GetExecutingAssembly();
        var resourceRoot = "EventHorizon.Zone.Core.Tests";
        var resourcePath = "FileService.App_Data";
        var resourceFile = "EmbeddedResource.txt";
        var saveFileFullName = Path.Combine(
            "the",
            "path",
            "to",
            "EmbeddedResource.txt"
        );

        var fileResolverMock = new Mock<FileResolver>();
        var directoryResolverMock = new Mock<DirectoryResolver>();

        fileResolverMock.Setup(
            mock => mock.DoesFileExist(
                saveFileFullName
            )
        ).Returns(
            true
        );

        // When
        var handler = new WriteResourceToFileHandler(
            fileResolverMock.Object,
            directoryResolverMock.Object
        );
        var actual = await handler.Handle(
            new WriteResourceToFile(
                assembly,
                resourceRoot,
                resourcePath,
                resourceFile,
                saveFileFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Fact]
    public async Task ShouldReturnResourceNotFoundWhenResourceIsNotFoundINExecutingAssembly()
    {
        // Given
        var expected = "resource_not_found";

        var assembly = Assembly.GetExecutingAssembly();
        var resourceRoot = "EventHorizon.Zone.Core.Tests";
        var resourcePath = "FileService.App_Data";
        var resourceFile = "Not_Existing_EmbeddedResource.txt";
        var saveFileFullName = Path.Combine(
            "the",
            "path",
            "to",
            "Not_Existing_EmbeddedResource.txt"
        );

        var fileResolverMock = new Mock<FileResolver>();
        var directoryResolverMock = new Mock<DirectoryResolver>();

        fileResolverMock.Setup(
            mock => mock.DoesFileExist(
                saveFileFullName
            )
        ).Returns(
            false
        );

        // When
        var handler = new WriteResourceToFileHandler(
            fileResolverMock.Object,
            directoryResolverMock.Object
        );
        var actual = await handler.Handle(
            new WriteResourceToFile(
                assembly,
                resourceRoot,
                resourcePath,
                resourceFile,
                saveFileFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }
}
