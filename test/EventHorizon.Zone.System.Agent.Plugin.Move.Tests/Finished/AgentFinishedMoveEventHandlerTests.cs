namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Finished
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Model.Entity.Client;
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
            var expected = ClientActionClientEntityStoppingToAllEvent.Create(
                new EntityClientStoppingData
                {
                    EntityId = entityId,
                }
            );

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
