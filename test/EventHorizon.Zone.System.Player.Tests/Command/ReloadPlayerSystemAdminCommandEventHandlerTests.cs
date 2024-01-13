namespace EventHorizon.Zone.System.Player.Tests.Command;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.Player.Command;
using EventHorizon.Zone.System.Player.Reload;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ReloadPlayerSystemAdminCommandEventHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldNotRunReloadWhenCommandIsNotReloadSystem(
        // Given
        AdminCommandEvent notification,
        [Frozen] Mock<IMediator> mediatorMock,
        ReloadPlayerSystemAdminCommandEventHandler handler
    )
    {
        // When
        await handler.Handle(
            notification,
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<ReloadPlayerSystemCommand>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldRunReloadCommandWhenCommandIsReloadSystem(
        // Given
        string connectionId,
        string rawCommand,
        List<string> commandParts,
        object data,
        [Frozen] Mock<IMediator> mediatorMock,
        ReloadPlayerSystemAdminCommandEventHandler handler
    )
    {
        // Given
        var command = "reload-system";

        // When
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
                It.IsAny<ReloadPlayerSystemCommand>(),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldSendResponseCommandWhenCommandIsReloadSystem(
        // Given
        string connectionId,
        string rawCommand,
        List<string> commandParts,
        object data,
        [Frozen] Mock<IMediator> mediatorMock,
        ReloadPlayerSystemAdminCommandEventHandler handler
    )
    {
        // Given
        var command = "reload-system";

        var responseMessage = "player_system_reloaded";

        var expected = new RespondToAdminCommand(
            connectionId,
            new StandardAdminCommandResponse(
                command,
                rawCommand,
                true,
                responseMessage
            )
        );

        // When
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
