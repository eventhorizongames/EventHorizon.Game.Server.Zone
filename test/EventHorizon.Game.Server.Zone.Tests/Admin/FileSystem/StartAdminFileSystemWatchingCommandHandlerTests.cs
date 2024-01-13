namespace EventHorizon.Game.Server.Zone.Tests.Admin.FileSystem;

using System;
using System.Collections.Generic;
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
        var coreMapPath = "core-map-path";

        var expectedServer = new StartWatchingFileSystemCommand(
            serverPath
        );
        var expectedAdmin = new StartWatchingFileSystemCommand(
            adminPath
        );
        var expectedI18n = new StartWatchingFileSystemCommand(
            i18nPath
        );
        var expectedAgent = new StartWatchingFileSystemCommand(
            Path.Combine(
                appDataPath,
                "Agent",
                "Reload"
            )
        );
        var expectedCoreMap = new StartWatchingFileSystemCommand(
            coreMapPath
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

        serverInfoMock.Setup(
            mock => mock.CoreMapPath
        ).Returns(
            coreMapPath
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

        var clientPaths = new List<string>
        {
            "Assets",
            "Entity",
            "Gui",
            "Modules",
            "Particle",
            "Scripts",
            "ServerModule",
            "Skills",
        };
        foreach (var clientSubPath in clientPaths)
        {
            mediatorMock.Verify(
                mock => mock.Send(
                     new StartWatchingFileSystemCommand(
                        Path.Combine(
                            clientPath,
                            clientSubPath
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        mediatorMock.Verify(
            mock => mock.Send(
                expectedAgent,
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mock => mock.Send(
                expectedCoreMap,
                CancellationToken.None
            )
        );
    }
}
