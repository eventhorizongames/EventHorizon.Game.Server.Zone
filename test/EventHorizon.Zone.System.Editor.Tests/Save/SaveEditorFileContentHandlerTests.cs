namespace EventHorizon.Zone.System.Editor.Tests.Save
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Editor.Events.Save;
    using EventHorizon.Zone.System.Editor.Save;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class SaveEditorFileContentHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenDeleteIsSuccessful()
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
            var fileDirectoryName = "file-directory-name";
            var fileExtension = "file-extension";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );
            var fileInfo = new StandardFileInfo(
                fileName,
                fileDirectoryName,
                fileFullName,
                fileExtension
            );

            var loggerMock = new Mock<ILogger<SaveEditorFileContentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetFileInfo(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileInfo
            );

            // When
            var handler = new SaveEditorFileContentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new SaveEditorFileContent(
                    filePath,
                    fileName,
                    fileContent
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeTrue();
            mediatorMock.Verify(
                mock => mock.Send(
                    new WriteAllTextToFile(
                        fileFullName,
                        fileContent
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldReturnTrueAndBackupFileContentWhenFileExists()
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
            var fileDirectoryName = "file-directory-name";
            var fileExtension = "file-extension";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );
            var fileInfo = new StandardFileInfo(
                fileName,
                fileDirectoryName,
                fileFullName,
                fileExtension
            );
            var fileExists = true;

            var loggerMock = new Mock<ILogger<SaveEditorFileContentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetFileInfo(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileInfo
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
            var handler = new SaveEditorFileContentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new SaveEditorFileContent(
                    filePath,
                    fileName,
                    fileContent
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeTrue();
            mediatorMock.Verify(
                mock => mock.Send(
                    new CreateBackupOfFileContentCommand(
                        filePath,
                        fileName,
                        fileContent
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldReturnFalseAndErrorCodeWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "server_exception";
            var appDataPath = "app-data-path";
            var filePath = new List<string>
            {
                "file",
                "path"
            };
            var fileName = "file-name";
            var fileContent = "file-content";
            var fileDirectoryName = "file-directory-name";
            var fileExtension = "file-extension";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );
            var fileInfo = new StandardFileInfo(
                fileName,
                fileDirectoryName,
                fileFullName,
                fileExtension
            );

            var loggerMock = new Mock<ILogger<SaveEditorFileContentHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<GetFileInfo>(),
                    CancellationToken.None
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var handler = new SaveEditorFileContentHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new SaveEditorFileContent(
                    filePath,
                    fileName,
                    fileContent
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }
    }
}
