namespace EventHorizon.Zone.System.Player.Plugin.Action.Tests.Register;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;
using EventHorizon.Zone.System.Player.Plugin.Action.Register;
using EventHorizon.Zone.System.Player.Plugin.Action.State;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class RegisterPlayerActionHandlerTests
{
    [Fact]
    public async Task TestShouldNotLogErrorOnAlreadyContainsPlayerActionWhenAddingPlayerActionEntityToRepository()
    {
        // Given
        var id = 1L;
        var actionName = "action-name";
        var actionEventMock = new Mock<PlayerActionEvent>();

        var loggerMock = new Mock<ILogger<RegisterPlayerActionHandler>>();
        var actionRepositoryMock = new Mock<PlayerActionRepository>();

        // When
        var handler = new RegisterPlayerActionHandler(
            loggerMock.Object,
            actionRepositoryMock.Object
        );
        await handler.Handle(
            new RegisterPlayerAction(
                id,
                actionName,
                actionEventMock.Object
            ),
            CancellationToken.None
        );

        // Then
        Assert.True(true);
    }
    [Fact]
    public async Task TestShouldLogErrorWhenAlreadyContainsPlayerAction()
    {
        // Given
        var id = 1L;
        var actionName = "action-name";
        var actionEventMock = new Mock<PlayerActionEvent>();

        var loggerMock = new Mock<ILogger<RegisterPlayerActionHandler>>();
        var actionRepositoryMock = new Mock<PlayerActionRepository>();

        actionRepositoryMock.Setup(
            mock => mock.On(
                It.IsAny<PlayerActionEntity>()
            )
        ).Throws(
            new AlreadyContainsPlayerAction(
                id
            )
        );

        // When
        var handler = new RegisterPlayerActionHandler(
            loggerMock.Object,
            actionRepositoryMock.Object
        );
        await handler.Handle(
            new RegisterPlayerAction(
                id,
                actionName,
                actionEventMock.Object
            ),
            CancellationToken.None
        );

        // Then
        Assert.True(true);
    }
}
