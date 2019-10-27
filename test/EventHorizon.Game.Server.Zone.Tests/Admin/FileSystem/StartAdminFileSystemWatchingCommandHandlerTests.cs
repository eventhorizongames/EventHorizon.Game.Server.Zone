using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.FileSystem;
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
        string appDataPath = IOPath.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Admin",
            "FileSystem"
        );
        string testingReloadPath = "";
        public StartAdminFileSystemWatchingCommandHandlerTests()
        {
            testingReloadPath = IOPath.Combine(
                appDataPath,
                "Agent",
                "Reload"
            );
            if (Directory.Exists(
                testingReloadPath
            ))
            {
                Directory.Delete(
                    testingReloadPath,
                    true
                );
            }
        }
        [Fact]
        public async Task TestShouldSendEventsForWatchingServerPathWhenEventIsHandled()
        {
            // Given
            var path = "path-to-server";
            var expected = new StartWatchingFileSystemCommand(
                path
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                ""
            );

            serverInfoMock.Setup(
                mock => mock.ServerPath
            ).Returns(
                path
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldSendEventsForWatchingAdminPathWhenEventIsHandled()
        {
            // Given
            var appDataPath = IOPath.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Admin",
                "FileSystem"
            );
            var path = IOPath.Combine(
                appDataPath,
                "Agent",
                "Reload"
            );
            var expected = new StartWatchingFileSystemCommand(
                path
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldCreateDirectoryForAgentReloadWhenStartingWatchOfPath()
        {
            // Given
            var path = testingReloadPath;

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            Assert.False(
                Directory.Exists(
                    path
                )
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            Assert.True(
                Directory.Exists(
                    path
                )
            );
        }

        [Fact]
        public async Task TestShouldSendEventsForWatchingI18nPathWhenEventIsHandled()
        {
            // Given
            var path = "path-to-i18n";
            var expected = new StartWatchingFileSystemCommand(
                path
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                ""
            );

            serverInfoMock.Setup(
                mock => mock.I18nPath
            ).Returns(
                path
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldSendEventsForWatchingClientPathWhenEventIsHandled()
        {
            // Given
            var path = "path-to-client";
            var expected = new StartWatchingFileSystemCommand(
                path
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                ""
            );

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                path
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldSendEventsForWatchingAgentReloadPathWhenEventIsHandled()
        {
            // Given
            var path = "path-to-app-data";
            var expected = new StartWatchingFileSystemCommand(
                System.IO.Path.Combine(
                    path,
                    "Agent",
                    "Reload"
                )
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                path
            );

            // When
            var handler = new StartAdminFileSystemWatchingCommandHandler(
                serverInfoMock.Object,
                mediatorMock.Object
            );
            await handler.Handle(
                new StartAdminFileSystemWatchingCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}