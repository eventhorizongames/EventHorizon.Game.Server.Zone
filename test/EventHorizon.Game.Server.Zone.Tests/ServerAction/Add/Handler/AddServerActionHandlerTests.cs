using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using EventHorizon.Game.Server.Zone.ServerAction.Add.Handler;
using EventHorizon.Game.Server.Zone.ServerAction.Model;
using System;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Events;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Add;
using EventHorizon.Zone.Core.Events.ServerAction;

namespace EventHorizon.Game.Server.Zone.Tests.ServerAction.Add.Handler
{
    public class AddServerActionHandlerTests
    {
        [Fact]
        public async Task TestHandler_ShouldAddPassedEventToPendingServerAction()
        {
            // Given
            var expectedRunAt = DateTime.Now;
            var expectedEvent = new TestNotificationEvent();
            var expectedServerActionEntity = new ServerActionEntity(expectedRunAt, expectedEvent);

            var inputAddServerActionEvent = new AddServerActionEvent(expectedRunAt, expectedEvent);

            var serverActionQueueMock = new Mock<IServerActionQueue>();

            // When
            var addServerActionHandler = new AddServerActionHandler(
                serverActionQueueMock.Object
            );

            await addServerActionHandler.Handle(inputAddServerActionEvent, CancellationToken.None);

            // Then
            serverActionQueueMock
                .Verify(
                    a => a.Push(
                        It.Is<ServerActionEntity>(entity => entity.RunAt.Equals(expectedRunAt) && entity.EventToSend.Equals(expectedEvent))
                )
            );
        }
    }
}