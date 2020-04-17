namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Finished
{
    using EventHorizon.Zone.Core.Events.Client.Actions;
    using EventHorizon.Zone.Core.Model.Client.DataType;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Finished;
    using global::System.Threading;
    using MediatR;
    using Moq;
    using Xunit;

    public class AgentFinishedMoveEventHandlerTests
    {
        [Fact]
        public void TestName()
        {
            //Given
            var entityId = 123L;
            var expected = new ClientActionClientEntityStoppingToAllEvent
            {
                Data = new EntityClientStoppingData
                {
                    EntityId = entityId,
                },
            };

            var mediatorMock = new Mock<IMediator>();

            //When
            var handler = new AgentFinishedMoveEventHandler(
                mediatorMock.Object
            );
            handler.Handle(
                new Events.AgentFinishedMoveEvent
                {
                    EntityId = entityId,
                },
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}