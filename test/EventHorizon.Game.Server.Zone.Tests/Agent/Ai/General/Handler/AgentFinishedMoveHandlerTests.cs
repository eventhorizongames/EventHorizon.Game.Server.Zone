using MediatR;
using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Handlers;
using EventHorizon.Game.Server.Zone.Agent.Events;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.General.Handler
{
    public class AgentFinishedMoveHandlerTests
    {
        [Fact]
        public async Task TestHandle()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var handler = new AgentFinishedMoveHandler(mediatorMock.Object);
            // When
            await handler.Handle(new AgentFinishedMoveEvent
            {
                EntityId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new AgentRoutineFinishedEvent
                {
                    EntityId = 123
                }, It.IsAny<CancellationToken>())
            );
        }
    }
}