namespace EventHorizon.Zone.System.Editor.Tests.Create
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Create;
    using EventHorizon.Zone.System.Editor.Events.Create;

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

    public class CreateEditorFolderHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueIfWhenTheFolderWasSuccessfulyCreated()
        {
            // Given
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var appDataPath = "app-data-path";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );

            var loggerMock = new Mock<ILogger<CreateEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateDirectory(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new CreateEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFolder(
                    folderPath,
                    folderName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFalseAndErrorCodeWhenDirectoryAlreadyExists()
        {
            // Given
            var expected = "folder_already_exists";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var appDataPath = "app-data-path";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );
            var directoryExists = true;

            var loggerMock = new Mock<ILogger<CreateEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                directoryExists
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new CreateEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFolder(
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
        public async Task ShouldReturnFalseAndErrorCodeWhenCreateDirectoryFailed()
        {
            // Given
            var expected = "folder_failed_to_create";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var appDataPath = "app-data-path";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );
            var directoryExists = false;
            var directoryCreated = false;

            var loggerMock = new Mock<ILogger<CreateEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                directoryExists
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new CreateDirectory(
                        folderFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                directoryCreated
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new CreateEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFolder(
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
        public async Task ShouldReturnFalseAndErrorCodeWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "server_exception";
            var folderPath = new List<string>
            {
                "folder",
                "path"
            };
            var folderName = "folder-name";
            var appDataPath = "app-data-path";
            var folderFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    folderPath.ToArray()
                ),
                folderName
            );

            var loggerMock = new Mock<ILogger<CreateEditorFolderHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
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
            var handler = new CreateEditorFolderHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFolder(
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
