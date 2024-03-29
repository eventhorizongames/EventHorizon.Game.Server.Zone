﻿namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Lifetime;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class OnStartupSetupAgentBehaviorPluginCommandHandlerTests
{
    [Fact]
    public async Task ShouldCreateBehaviorDirectoriesWhenDoesNotExist()
    {
        // Given
        var serverScriptsPath = "server-scripts-path";
        var serverPath = "server-path";
        var expectedList = new List<CreateDirectory>
        {
            new CreateDirectory(
                Path.Combine(
                    serverPath,
                    "Behaviors"
                )
            ),
            new CreateDirectory(
                Path.Combine(
                    serverScriptsPath,
                    "Behavior"
                )
            ),
        };

        var loggerMock = new Mock<ILogger<OnStartupSetupAgentBehaviorPluginCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.ServerScriptsPath
        ).Returns(
            serverScriptsPath
        );
        serverInfoMock.Setup(
            mock => mock.ServerPath
        ).Returns(
            serverPath
        );

        // When
        var handler = new OnStartupSetupAgentBehaviorPluginCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupAgentBehaviorPluginCommand(),
            CancellationToken.None
        );

        // Then
        actual.Should().Be(
            new OnServerStartupResult(
                true
            )
        );

        foreach (var expected in expectedList)
        {
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }

    [Fact]
    public async Task ShouldNotCreateBehaviorDirectoriesWhenAlreadyExisting()
    {
        // Given
        var serverScriptsPath = "server-scripts-path";
        var serverPath = "server-path";
        var behaviorScriptPath = Path.Combine(
            serverScriptsPath,
            "Behavior"
        );
        var serverBehaviorsPath = Path.Combine(
            serverPath,
            "Behaviors"
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupAgentBehaviorPluginCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.ServerScriptsPath
        ).Returns(
            serverScriptsPath
        );
        serverInfoMock.Setup(
            mock => mock.ServerPath
        ).Returns(
            serverPath
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    behaviorScriptPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );
        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    serverBehaviorsPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var handler = new OnStartupSetupAgentBehaviorPluginCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupAgentBehaviorPluginCommand(),
            CancellationToken.None
        );

        // Then
        actual.Should().Be(
            new OnServerStartupResult(
                true
            )
        );

        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<CreateDirectory>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }
}
