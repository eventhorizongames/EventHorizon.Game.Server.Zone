using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.FileSystem;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Watcher.Events.Start;
using MediatR;
using Moq;
using Xunit;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Tests.Admin.FileSystem
{
    public class StartAdminFileSystemWatchingCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldSendEventsForWatchingServerPathWhenEventIsHandled()
        {
            // Given
            var serverPath = "path-to-server";
            var adminPath = "path-to-admin";
            var i18nPath = "path-to-i18n";
            var clientPath = "path-to-client";
            var appDataPath = "path-to-app-data";

            var expectedServer = new StartWatchingFileSystemCommand(
                serverPath
            );
            var expectedAdmin = new StartWatchingFileSystemCommand(
                adminPath
            );
            var expectedI18n = new StartWatchingFileSystemCommand(
                i18nPath
            );
            var expectedClient = new StartWatchingFileSystemCommand(
                clientPath
            );
            var expectedAgent = new StartWatchingFileSystemCommand(
                IOPath.Combine(
                    appDataPath,
                    "Agent",
                    "Reload"
                )
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.ServerPath
            ).Returns(
                serverPath
            );

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );

            serverInfoMock.Setup(
                mock => mock.I18nPath
            ).Returns(
                i18nPath
            );

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );


            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedServer,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedAdmin,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedI18n,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedClient,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedAgent,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldCreateDirectoryForAgentReloadWhenAgentReloadDirectoryDoesNotExist()
        {
            // Given
            var appDataPath = IOPath.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Admin",
                "FileSystem"
            );
            var agentReloadPath = IOPath.Combine(
                appDataPath,
                "Agent",
                "Reload"
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        agentReloadPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new CreateDirectory(
                        agentReloadPath
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}