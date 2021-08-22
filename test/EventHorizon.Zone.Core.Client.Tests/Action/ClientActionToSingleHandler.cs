namespace EventHorizon.Zone.Core.Client.Tests.Action
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Client.Action;
    using EventHorizon.Zone.Core.Events.Client;
    using EventHorizon.Zone.Core.Model.Client;

    using MediatR;

    using Moq;

    using Xunit;

    public class ClientActionToSingleHandlerTests
    {
        [Fact]
        public async Task TestShouldSendToSingleClientEventWhenEventIsHandled()
        {
            // Given
            var connectionId = "connection-id";
            var action = "action-to-send";
            var data = new TestClientActionData();
            var inputEvent = new TestClientActionEvent(
                connectionId,
                action,
                data
            );
            var expected = new SendToSingleClientEvent
            {
                ConnectionId = connectionId,
                Method = "ClientAction",
                Arg1 = action,
                Arg2 = data
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ClientActionToSingleHandler<TestClientActionEvent, TestClientActionData>(
                mediatorMock.Object
            );
            await handler.Handle(
                inputEvent,
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        public class TestClientActionEvent : ClientActionToSingleEvent<TestClientActionData>
        {
            private string _connectionId;
            private string _action;
            private TestClientActionData _data;
            public TestClientActionEvent(
                string connectionId,
                string action,
                TestClientActionData data
            )
            {
                _connectionId = connectionId;
                _action = action;
                _data = data;
            }
            public override string ConnectionId
            {
                get => _connectionId;
            }
            public override string Action => _action;
            public override TestClientActionData Data
            {
                get => _data;
            }
        }
        public class TestClientActionData : IClientActionData
        {

        }
    }
}
