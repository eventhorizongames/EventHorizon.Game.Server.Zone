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

    public class ClientActionToAllHandlerTests
    {
        [Fact]
        public async Task TestShouldSendToAllClientsEventWhenEventIsHandled()
        {
            // Given
            var action = "action-to-send";
            var data = new TestClientActionData();
            var inputEvent = new TestClientActionEvent(
                action,
                data
            );
            var expected = new SendToAllClientsEvent
            {
                Method = "ClientAction",
                Arg1 = action,
                Arg2 = data
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ClientActionToAllHandler<TestClientActionEvent, TestClientActionData>(
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

        public class TestClientActionEvent : ClientActionToAllEvent<TestClientActionData>
        {
            private readonly string _action;
            private readonly TestClientActionData _data;

            public override string Action => _action;
            public override TestClientActionData Data
            {
                get => _data;
            }

            public TestClientActionEvent(
                string action,
                TestClientActionData data
            )
            {
                _action = action;
                _data = data;
            }
        }
        public class TestClientActionData : IClientActionData
        {

        }
    }
}
