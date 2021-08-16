namespace EventHorizon.Zone.System.Wizard.Tests.Command
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Wizard.Command;
    using EventHorizon.Zone.System.Wizard.Load;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class ReloadWizardSystemAdminCommandEventHandlerTests
    {
        [Fact]
        public async Task ShouldIgnoreReloadingSystemWhenCommandIsNotSystemReload()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "not-reload-system";
            var commandParts = new List<string>();
            var data = new { };

            var expected = new LoadWizardListCommand();
            var expectedSystemCommand = new LoadSystemsWizardListCommand();

            var loggerMock = new Mock<ILogger<ReloadWizardSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadWizardSystemAdminCommandEventHandler(
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
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedSystemCommand,
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldSendLoadSystemsWizardListCommandWhenCommandIsSystemReload()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "reload-system";
            var commandParts = new List<string>();
            var data = new { };

            var expected = new LoadSystemsWizardListCommand();

            var loggerMock = new Mock<ILogger<ReloadWizardSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadWizardSystemAdminCommandEventHandler(
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
        public async Task ShouldSendLoadWizardListCommandWhenCommandIsSystemReload()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "reload-system";
            var commandParts = new List<string>();
            var data = new { };

            var expected = new LoadWizardListCommand();

            var loggerMock = new Mock<ILogger<ReloadWizardSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadWizardSystemAdminCommandEventHandler(
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
        public async Task ShouldResponseToClientWhenCommandIsSystemReloadAndSuccessful()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "reload-system";
            var commandParts = new List<string>();
            var data = new { };
            var responseMessage = "wizard_system_reloaded";

            var expected = new SendAdminCommandResponseToAllCommand(
                new StandardAdminCommandResponse(
                    command,
                    rawCommand,
                    true,
                    responseMessage
                )
            );

            var loggerMock = new Mock<ILogger<ReloadWizardSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadSystemsWizardListCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadWizardListCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult()
            );

            // When
            var handler = new ReloadWizardSystemAdminCommandEventHandler(
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
        public async Task ShouldNotResponseToClientWhenLoadWizardListCommandFails()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "reload-system";
            var commandParts = new List<string>();
            var data = new { };
            var responseMessage = "wizard_system_not_reloaded";

            var expected = new RespondToAdminCommand(
                connectionId,
                new StandardAdminCommandResponse(
                    command,
                    rawCommand,
                    false,
                    responseMessage
                )
            );

            var loggerMock = new Mock<ILogger<ReloadWizardSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<LoadSystemsWizardListCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new StandardCommandResult(
                    "Error-Code"
                )
            );

            // When
            var handler = new ReloadWizardSystemAdminCommandEventHandler(
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
    }
}
