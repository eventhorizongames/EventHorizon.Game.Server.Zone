using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Handlers;
using EventHorizon.Game.Server.Zone.Agent.Events;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.General.Handler
{
    public class AgentRoutineFinishedHandlerTests
    {
        [Fact]
        public async Task TestHandle()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(123)).ReturnsAsync(new AgentEntity
            {
                Id = 123
            });
            var handler = new AgentRoutineFinishedHandler(mediatorMock.Object, agentRepositoryMock.Object);
            // When
            await handler.Handle(new AgentRoutineFinishedEvent
            {
                EntityId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new ClearAgentRoutineEvent
                {
                    EntityId = 123
                }, It.IsAny<CancellationToken>())
            );
            mediatorMock.Verify(mediator => mediator.Publish(
                new RunAgentDefaultRoutineEvent
                {
                    EntityId = 123
                }, It.IsAny<CancellationToken>())
            );
        }
    }
}