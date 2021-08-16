namespace EventHorizon.Zone.System.ClientAssets.Tests.Command
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.ClientAssets.Command;
    using EventHorizon.Zone.System.ClientAssets.Load;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class ReloadClientAssetsSystemAdminCommandEventHandlerTests
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

            var expected = new LoadSystemClientAssetsCommand();

            var loggerMock = new Mock<ILogger<ReloadClientAssetsSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadClientAssetsSystemAdminCommandEventHandler(
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

            var expected = new LoadSystemClientAssetsCommand();

            var loggerMock = new Mock<ILogger<ReloadClientAssetsSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadClientAssetsSystemAdminCommandEventHandler(
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
        public async Task ShouldSendResponseCommandWhenCommandIsReloadSystem()
        {
            // Given
            var connectionId = "command-id";
            var rawCommand = "raw-command";
            var command = "reload-system";
            var commandParts = new List<string>();
            var data = new { };
            var responseMessage = "client_assets_system_reloaded";

            var expected = new RespondToAdminCommand(
                connectionId,
                new StandardAdminCommandResponse(
                    command,
                    rawCommand,
                    true,
                    responseMessage
                )
            );

            var loggerMock = new Mock<ILogger<ReloadClientAssetsSystemAdminCommandEventHandler>>();
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadClientAssetsSystemAdminCommandEventHandler(
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
