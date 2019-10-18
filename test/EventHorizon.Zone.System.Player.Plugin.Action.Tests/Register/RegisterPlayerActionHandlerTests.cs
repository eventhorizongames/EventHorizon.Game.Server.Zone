using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;
using EventHorizon.Zone.System.Player.Plugin.Action.Register;
using EventHorizon.Zone.System.Player.Plugin.Action.State;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Player.Plugin.Action.Tests.Register
{
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
            loggerMock.Verify(
                mock => mock.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<FormattedLogValues>(),
                    It.IsAny<AlreadyContainsPlayerAction>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Never()
            );
        }
        [Fact]
        public async Task TestShouldLogErrorWhenAlreadyContainsPlayerAction()
        {
            // Given
            var id = 1L;
            var actionName = "action-name";
            var actionEventMock = new Mock<PlayerActionEvent>();
            var expected = "Action Repository already contains a copy of Player Action: \n | Id: 1 \n | Name: action-name";

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
            loggerMock.Verify(
                mock => mock.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(
                        v => v.ToString().Contains(
                            expected
                        )
                    ),
                    It.IsAny<AlreadyContainsPlayerAction>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }
    }
}