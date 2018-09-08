using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using System.Collections;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.ServerAction.Model;
using System;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Events;
using MediatR;
using EventHorizon.Game.Server.Zone.ServerAction.Run.Handler;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Run;

namespace EventHorizon.Game.Server.Zone.Tests.ServerAction.Run.Handler
{
    public class RunPendingServerActionsHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishExpectedEventsWhenReturnedFromQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            var expectedActionToSend1 = new TestNotificationEvent();
            var expectedActionToSend2 = new TestNotificationEvent();
            var expectedActionToSend3 = new TestNotificationEvent();
            var action1 = new ServerActionEntity(DateTime.Now, expectedActionToSend1);
            var action2 = new ServerActionEntity(DateTime.Now, expectedActionToSend2);
            var action3 = new ServerActionEntity(DateTime.Now, expectedActionToSend3);
            var serverActionList = new List<ServerActionEntity>()
            {
                action1,
                action2,
                action3
            };

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(a => a.Take(expectedActionToTake)).ReturnsAsync(serverActionList);

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(inputEvent, CancellationToken.None);

            // Then
            serverActionQueueMock.Verify(a => a.Take(expectedActionToTake));
            mediatorMock.Verify(a => a.Publish<INotification>(expectedActionToSend1, CancellationToken.None));
            mediatorMock.Verify(a => a.Publish<INotification>(expectedActionToSend2, CancellationToken.None));
            mediatorMock.Verify(a => a.Publish<INotification>(expectedActionToSend3, CancellationToken.None));
        }
        [Fact]
        public async Task TestHandle_ShouldNotThrowExceptionOnEmptyQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            var serverActionList = new List<ServerActionEntity>();

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(a => a.Take(expectedActionToTake)).ReturnsAsync(serverActionList);

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(inputEvent, CancellationToken.None);

            // Then
            serverActionQueueMock.Verify(a => a.Take(expectedActionToTake));
        }
        [Fact]
        public async Task TestHandle_ShouldNotThrowExceptionOnNullQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            List<ServerActionEntity> serverActionList = null;

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(a => a.Take(expectedActionToTake)).ReturnsAsync(serverActionList);

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(inputEvent, CancellationToken.None);

            // Then
            serverActionQueueMock.Verify(a => a.Take(expectedActionToTake));
        }
    }
}