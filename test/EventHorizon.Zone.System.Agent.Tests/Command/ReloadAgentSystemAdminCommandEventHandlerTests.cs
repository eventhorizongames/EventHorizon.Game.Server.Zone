namespace EventHorizon.Zone.System.Agent.Tests.Command;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.Agent.Command;
using EventHorizon.Zone.System.Agent.Reload;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class ReloadAgentSystemAdminCommandEventHandlerTests
{
    [Fact]
    public async Task ShouldNotRunReloadWhenCommandIsNotReloadSystem()
    {
        // Given
        var connectionId = "command-id";
        var rawCommand = "raw-command";
        var command = "not-reload-system";
        var commandParts = new List<string>();
        var data = new { };

        var expected = new ReloadAgentSystemCommand();

        var loggerMock = new Mock<ILogger<ReloadAgentSystemAdminCommandEventHandler>>();
        var mediatorMock = new Mock<IMediator>();

        // When
        var handler = new ReloadAgentSystemAdminCommandEventHandler(
            loggerMock.Object,
            mediatorMock.Object
        );
        await handler.Handle(
            new AdminCommandEvent(
                connectionId,
                new StandardAdminCommand(
                    rawCommand,
                    command,
                    commandParts
                ),
                data
            ),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                expected,
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Fact]
    public async Task ShouldRunReloadCommandWhenCommandIsReloadSystem()
    {
        // Given
        var connectionId = "command-id";
        var rawCommand = "raw-command";
        var command = "reload-system";
        var commandParts = new List<string>();
        var data = new { };

        var expected = new ReloadAgentSystemCommand();

        var loggerMock = new Mock<ILogger<ReloadAgentSystemAdminCommandEventHandler>>();
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new ReloadAgentSystemCommand(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult()
        );

        // When
        var handler = new ReloadAgentSystemAdminCommandEventHandler(
            loggerMock.Object,
            mediatorMock.Object
        );
        await handler.Handle(
            new AdminCommandEvent(
                connectionId,
                new StandardAdminCommand(
                    rawCommand,
                    command,
                    commandParts
                ),
                data
            ),
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
    public async Task ShouldSendResponseCommandWhenCommandIsReloadSystemAndReloadAgentIsSuccessful()
    {
        // Given
        var connectionId = "command-id";
        var rawCommand = "raw-command";
        var command = "reload-system";
        var commandParts = new List<string>();
        var data = new { };
        var responseMessage = "agent_system_reloaded";

        var expected = new RespondToAdminCommand(
            connectionId,
            new StandardAdminCommandResponse(
                command,
                rawCommand,
                true,
                responseMessage
            )
        );

        var loggerMock = new Mock<ILogger<ReloadAgentSystemAdminCommandEventHandler>>();
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new ReloadAgentSystemCommand(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult()
        );

        // When
        var handler = new ReloadAgentSystemAdminCommandEventHandler(
            loggerMock.Object,
            mediatorMock.Object
        );
        await handler.Handle(
            new AdminCommandEvent(
                connectionId,
                new StandardAdminCommand(
                    rawCommand,
                    command,
                    commandParts
                ),
                data
            ),
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
    public async Task ShouldNotSendResponseCommandWhenCommandIsReloadSystemAndReloadAgentIsNotSuccessful()
    {
        // Given
        var connectionId = "command-id";
        var rawCommand = "raw-command";
        var command = "reload-system";
        var commandParts = new List<string>();
        var data = new { };

        var loggerMock = new Mock<ILogger<ReloadAgentSystemAdminCommandEventHandler>>();
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new ReloadAgentSystemCommand(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult(
                "error-code"
            )
        );

        // When
        var handler = new ReloadAgentSystemAdminCommandEventHandler(
            loggerMock.Object,
            mediatorMock.Object
        );
        await handler.Handle(
            new AdminCommandEvent(
                connectionId,
                new StandardAdminCommand(
                    rawCommand,
                    command,
                    commandParts
                ),
                data
            ),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<RespondToAdminCommand>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }
}
