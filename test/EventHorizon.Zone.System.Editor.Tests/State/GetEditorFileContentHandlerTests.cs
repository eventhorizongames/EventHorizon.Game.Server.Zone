namespace EventHorizon.Zone.System.Editor.Tests.State
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events.State;
    using EventHorizon.Zone.System.Editor.Model;
    using EventHorizon.Zone.System.Editor.State;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class GetEditorFileContentHandlerTests
    {
        [Fact]
        public async Task ShoudlReturnExpectedStandardEditorFileWhenFileExists()
        {
            // Given
            var appDataPath = "app-data-path";
            var filePath = new List<string>
            {
                "file",
                "path"
            };
            var fileName = "file-name";
            var fileContent = "file-content";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );
            var fileId = StandardEditorFile.GenerateId(
                fileName,
                filePath
            );
            var fileExists = true;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileExists
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileContent
            );

            // When
            var handler = new GetEditorFileContentHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new GetEditorFileContent(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Id
                .Should().Be(fileId);
            actual.Name
                .Should().Be(fileName);
            actual.Path
                .Should().BeEquivalentTo(filePath);
            actual.Content
                .Should().Be(fileContent);
        }

        [Fact]
        public async Task ShoudlReturnInvalidStandardEditorFileWhenFileDoesNotExists()
        {
            // Given
            var expectedFileName = GetEditorFileContentHandler.INVALID_FILE_IDENTIFIER;
            var expectedFilePath = new List<string>
            {
                GetEditorFileContentHandler.INVALID_FILE_IDENTIFIER
            };
            var expectedFileContent = GetEditorFileContentHandler.INVALID_FILE_IDENTIFIER;
            var expectedFileId = StandardEditorFile.GenerateId(
                expectedFileName,
                expectedFilePath
            );
            var fileExists = false;
            var appDataPath = "app-data-path";
            var filePath = new List<string>
            {
                "file",
                "path"
            };
            var fileName = "file-name";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );
            var fileId = StandardEditorFile.GenerateId(
                fileName,
                filePath
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileExists
            );

            // When
            var handler = new GetEditorFileContentHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new GetEditorFileContent(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Id
                .Should().Be(expectedFileId);
            actual.Name
                .Should().Be(expectedFileName);
            actual.Path
                .Should().BeEquivalentTo(expectedFilePath);
            actual.Content
                .Should().Be(expectedFileContent);
        }
    }
}
