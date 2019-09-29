using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Interaction.Client.Messsage;
using EventHorizon.Zone.System.Interaction.Events.Client;
using EventHorizon.Zone.System.Interaction.Model.Client;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Interaction.Tests.Client.Message
{
    public class SendSingleInteractionClientActionEventHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishExpectedMessage()
        {
            // Given
            var connectionId = "connection-id";
            var data = new InteractionClientActionData(
                "commandType",
                new { }
            );
            var expectedAction = "SERVER_INTERACTION_CLIENT_ACTION_EVENT";
            var expectedEvent = new SendToSingleClientEvent
            {
                ConnectionId = connectionId,
                Method = "ClientAction",
                Arg1 = expectedAction,
                Arg2 = data
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new SendSingleInteractionClientActionEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new SendSingleInteractionClientActionEvent(
                    connectionId,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedEvent,
                    CancellationToken.None
                )
            );
        }
    }
}