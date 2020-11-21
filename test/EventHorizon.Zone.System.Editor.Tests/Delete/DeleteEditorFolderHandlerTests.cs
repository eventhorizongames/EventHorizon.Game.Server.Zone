namespace EventHorizon.Zone.System.Editor.Tests.Delete
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
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

    public class DeleteEditorFolderHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenDeleteIsSuccessful()
        {
            // Given
            var appDataPath = "app-data-path";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );
            var deletedDirectory = true;

            var loggerMock = new Mock<ILogger<DeleteEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsDirectoryEmpty(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DeleteDirectory(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                deletedDirectory
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFolder(
                    folderPath,
                    folderName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFailureAndErrorCodeWhenDirectoryIsNotSuccessfulyDeleted()
        {
            // Given
            var expected = "folder_failed_to_delete";
            var appDataPath = "app-data-path";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );
            var deletedDirectory = false;

            var loggerMock = new Mock<ILogger<DeleteEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsDirectoryEmpty(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DeleteDirectory(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                deletedDirectory
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFolder(
                    folderPath,
                    folderName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnFailureAndErrorCodeWhenDirectoryIsNotEmpty()
        {
            // Given
            var expected = "folder_not_empty";
            var appDataPath = "app-data-path";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );
            var directoryEmpty = false;

            var loggerMock = new Mock<ILogger<DeleteEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsDirectoryEmpty(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                directoryEmpty
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new DeleteEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFolder(
                    folderPath,
                    folderName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnFailureAndErrorCodeWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "server_exception";
            var appDataPath = "app-data-path";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );

            var loggerMock = new Mock<ILogger<DeleteEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new IsDirectoryEmpty(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DeleteDirectory(
                        folderFullName
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
            var handler = new DeleteEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new DeleteEditorFolder(
                    folderPath,
                    folderName
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
