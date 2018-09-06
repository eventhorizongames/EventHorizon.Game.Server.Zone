using EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler;
using MediatR;
using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;

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
                AgentId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new AgentRoutineFinishedEvent
                {
                    AgentId = 123
                }, It.IsAny<CancellationToken>())
            );
        }
    }
}