namespace EventHorizon.Zone.System.Editor.Tests.Delete
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Editor.Delete;
    using EventHorizon.Zone.System.Editor.Events.Delete;
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

    public class DeleteEditorFileHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenFileExists()
        {
            // Given
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


            var loggerMock = new Mock<ILogger<DeleteEditorFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFile(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeTrue();
            actual.ErrorCode
                .Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldCreateBackupOfFileWhenFileExists()
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
            var fileExists = true;


            var loggerMock = new Mock<ILogger<DeleteEditorFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

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

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFile(
                    filePath,
                    fileName
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
        public async Task ShouldReturnFailureWhenAnyExceptionIsThrown()
        {
            // Given
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

            var loggerMock = new Mock<ILogger<DeleteEditorFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).Throws(
                new Exception("error")
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFile(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeFalse();
        }
    }
}
