using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.Core.ServerAction.Model;
using EventHorizon.Zone.Core.ServerAction.Run;
using EventHorizon.Zone.Core.ServerAction.State;

using MediatR;

using Moq;

using Xunit;

namespace EventHorizon.Zone.Core.ServerAction.Tests.Run
{
    public class RunPendingServerActionsHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishExpectedEventsWhenReturnedFromQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            var expectedActionToSend1 = new TestNotificationEvent();
            var expectedActionToSend2 = new TestNotificationEvent();
            var expectedActionToSend3 = new TestNotificationEvent();
            var action1 = new ServerActionEntity(
                DateTime.Now,
                expectedActionToSend1
            );
            var action2 = new ServerActionEntity(
                DateTime.Now,
                expectedActionToSend2
            );
            var action3 = new ServerActionEntity(
                DateTime.Now,
                expectedActionToSend3
            );
            var serverActionList = new List<ServerActionEntity>()
            {
                action1,
                action2,
                action3
            };

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(
                mock => mock.Take(
                    expectedActionToTake
                )
            ).Returns(
                serverActionList
            );

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(
                inputEvent,
                CancellationToken.None
            );

            // Then
            serverActionQueueMock.Verify(
                mock => mock.Take(expectedActionToTake)
            );
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedActionToSend1, CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedActionToSend2,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedActionToSend3,
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldNotThrowExceptionOnEmptyQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            var serverActionList = new List<ServerActionEntity>();

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(
                mock => mock.Take(
                    expectedActionToTake
                )
            ).Returns(
                serverActionList
            );

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(
                inputEvent,
                CancellationToken.None
            );

            // Then
            serverActionQueueMock.Verify(
                mock => mock.Take(
                    expectedActionToTake
                )
            );
        }
        [Fact]
        public async Task TestShouldNotThrowExceptionOnNullQueue()
        {
            // Given
            var inputEvent = new RunPendingServerActionsEvent();

            var expectedActionToTake = 10;
            List<ServerActionEntity> serverActionList = null;

            var mediatorMock = new Mock<IMediator>();
            var serverActionQueueMock = new Mock<IServerActionQueue>();

            serverActionQueueMock.Setup(
                mock => mock.Take(
                    expectedActionToTake
                )
            ).Returns(
                serverActionList
            );

            // When
            var runPendingServerActionsHandler = new RunPendingServerActionsHandler(
                mediatorMock.Object,
                serverActionQueueMock.Object
            );

            await runPendingServerActionsHandler.Handle(
                inputEvent,
                CancellationToken.None
            );

            // Then
            serverActionQueueMock.Verify(
                mock => mock.Take(
                    expectedActionToTake
                )
            );
        }
    }
}
