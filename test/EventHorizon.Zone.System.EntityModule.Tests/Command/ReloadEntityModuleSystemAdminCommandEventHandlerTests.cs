namespace EventHorizon.Zone.System.EntityModule.Tests.Command;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.EntityModule.Command;
using EventHorizon.Zone.System.EntityModule.Reload;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ReloadEntityModuleSystemAdminCommandEventHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldNotRunReloadWhenCommandIsNotReloadSystem(
        // Given
        string connectionId, 
        string rawCommand,
        string command,
        List<string> commandParts,
        object data,
        [Frozen] Mock<IMediator> mediatorMock,
        ReloadEntityModuleSystemAdminCommandEventHandler handler
    )
    {
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
            mock => mock.Publish(
                It.IsAny<INotification>(),
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
        ReloadEntityModuleSystemAdminCommandEventHandler handler
    )
    {
        var command = "reload-system";

        var expected = new ReloadEntityModuleSystemCommand();

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

    [Theory, AutoMoqData]
    public async Task ShouldSendResponseCommandWhenCommandIsReloadSystem(
        // Given
        string connectionId,
        string rawCommand,
        List<string> commandParts,
        object data,
        [Frozen] Mock<IMediator> mediatorMock,
        ReloadEntityModuleSystemAdminCommandEventHandler handler
    )
    {
        var command = "reload-system";
        var responseMessage = "entity_module_system_reloaded";

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
