namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Reload
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Reload;

    using MediatR;

    using Moq;

    using Xunit;

    public class ReloadAdminCommandsHandlerTests
    {
        [Fact]
        public async Task TestShouldSendLoadAdminCommandsWhenCommandIsReloadAdmin()
        {
            // Given
            var expected = new LoadAdminCommands();
            var connectionId = "connection-id";
            var command = "reload-admin";
            var data = new { };

            var mediatorMock = new Mock<IMediator>();
            var commandMock = new Mock<IAdminCommand>();

            commandMock.Setup(
                mock => mock.Command
            ).Returns(
                command
            );

            // When
            var handler = new ReloadAdminCommandsHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    commandMock.Object,
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
        public async Task TestShouldSendResponseToConnectionIdWhenCommandIsValid()
        {
            // Given
            var connectionId = "connection-id";
            var rawCommand = "raw-command";
            var command = "reload-admin";
            var data = new { };
            var expected = new RespondToAdminCommand(
                connectionId,
                new StandardAdminCommandResponse(
                    command,
                    rawCommand,
                    true,
                    "reload_admin_successful"
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var commandMock = new Mock<IAdminCommand>();

            commandMock.Setup(
                mock => mock.Command
            ).Returns(
                command
            );
            commandMock.Setup(
                mock => mock.RawCommand
            ).Returns(
                rawCommand
            );

            // When
            var handler = new ReloadAdminCommandsHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    commandMock.Object,
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
        public async Task TestShouldNotSendCommandsWhenCommandIsNotValid()
        {
            // Given
            var command = "not-expected-command";

            var mediatorMock = new Mock<IMediator>();
            var commandMock = new Mock<IAdminCommand>();

            commandMock.Setup(
                mock => mock.Command
            ).Returns(
                command
            );

            // When
            var handler = new ReloadAdminCommandsHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    null,
                    commandMock.Object,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<IRequest>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
