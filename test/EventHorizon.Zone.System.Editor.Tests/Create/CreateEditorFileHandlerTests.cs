namespace EventHorizon.Zone.System.Editor.Tests.Create
{
    using EventHorizon.Zone.Core.Events.FileService;
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

    public class CreateEditorFileHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenFileDoesNotExist()
        {
            // Given
            var fileName = "file-name";
            var filePath = new List<string> { "path" };
            var appDataPath = "app-data-path";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );

            var loggerMock = new Mock<ILogger<CreateEditorFileHandler>>();
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
                false
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new CreateEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFile(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ShouldErrorCodeOfFileAlreadyExistsWhenDoesFileExistReturnsTrue()
        {
            // Given
            var expected = "file_already_exists";
            var fileName = "file-name";
            var filePath = new List<string> { "path" };
            var appDataPath = "app-data-path";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );

            var loggerMock = new Mock<ILogger<CreateEditorFileHandler>>();
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
                true
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new CreateEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFile(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should()
                .BeFalse();
            actual.ErrorCode
                .Should()
                .Be(expected);
        }

        [Fact]
        public async Task ShouldErrorCodeOfServerExceptionWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "server_exception";
            var fileName = "file-name";
            var filePath = new List<string> { "path" };
            var appDataPath = "app-data-path";
            var fileFullName = Path.Combine(
                appDataPath,
                Path.Combine(
                    filePath.ToArray()
                ),
                fileName
            );

            var loggerMock = new Mock<ILogger<CreateEditorFileHandler>>();
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
            var handler = new CreateEditorFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateEditorFile(
                    filePath,
                    fileName
                ),
                CancellationToken.None
            );

            // Then
            actual.Successful
                .Should()
                .BeFalse();
            actual.ErrorCode
                .Should()
                .Be(expected);
        }
    }
}
