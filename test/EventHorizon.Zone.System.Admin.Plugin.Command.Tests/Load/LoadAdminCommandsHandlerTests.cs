namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Load
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
    using EventHorizon.Zone.System.Admin.Plugin.Command.State;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadAdminCommandsHandlerTests
    {
        [Fact]
        public async Task TestShouldAddAdminCommandFromAdminCommandsPath()
        {
            // Given
            var expected = new AdminCommandInstance();
            var fileFullName = "file-full-name";
            var adminPath = "admin-path";
            var directoryFullName = Path.Combine(
                adminPath,
                "Commands"
            );
            var fileInfoList = new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    fileFullName,
                    "directory-name",
                    "full-name",
                    "extensions"
                )
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var repositoryMock = new Mock<AdminCommandRepository>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        directoryFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileInfoList
            );

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<AdminCommandInstance>(
                    fileFullName
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var handler = new LoadAdminCommandsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                repositoryMock.Object,
                fileLoaderMock.Object
            );
            await handler.Handle(
                new LoadAdminCommands(),
                CancellationToken.None
            );

            // Then
            repositoryMock.Verify(
                mock => mock.Add(
                    expected
                )
            );
        }

        [Fact]
        public async Task TestShouldNotAddAnyAdminCommandWhenTheDirectoryIsEmpty()
        {
            // Given
            var adminPath = "admin-path";
            var directoryFullName = Path.Combine(
                adminPath,
                "Commands"
            );
            var fileInfoList = new List<StandardFileInfo>();

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var repositoryMock = new Mock<AdminCommandRepository>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        directoryFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileInfoList
            );

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );

            // When
            var handler = new LoadAdminCommandsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                repositoryMock.Object,
                fileLoaderMock.Object
            );
            await handler.Handle(
                new LoadAdminCommands(),
                CancellationToken.None
            );

            // Then
            repositoryMock.Verify(
                mock => mock.Add(
                    It.IsAny<AdminCommandInstance>()
                ),
                Times.Never()
            );
        }
    }
}
