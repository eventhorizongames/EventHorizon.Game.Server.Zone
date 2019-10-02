using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.ServerAction;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.Core.ServerAction.Model;
using EventHorizon.Zone.Core.ServerAction.State;
using EventHorizon.Zone.Core.ServerAction.ServerAction.Add;

namespace EventHorizon.Zone.Core.ServerAction.Tests.Add
{
    public class AddServerActionHandlerTests
    {
        [Fact]
        public async Task TestShouldAddPassedEventToPendingServerAction()
        {
            // Given
            var expectedRunAt = DateTime.Now;
            var expectedEvent = new TestNotificationEvent();
            var expectedServerActionEntity = new ServerActionEntity(
                expectedRunAt, 
                expectedEvent
            );

            var inputAddServerActionEvent = new AddServerActionEvent(
                expectedRunAt, 
                expectedEvent
            );

            var serverActionQueueMock = new Mock<IServerActionQueue>();

            // When
            var addServerActionHandler = new AddServerActionHandler(
                serverActionQueueMock.Object
            );

            await addServerActionHandler.Handle(
                inputAddServerActionEvent, 
                CancellationToken.None
            );

            // Then
            serverActionQueueMock.Verify(
                mock => mock.Push(
                    It.Is<ServerActionEntity>(
                        entity => entity.RunAt.Equals(
                            expectedRunAt
                        ) && entity.EventToSend.Equals(
                            expectedEvent
                        )
                    )
                )
            );
        }
    }
}