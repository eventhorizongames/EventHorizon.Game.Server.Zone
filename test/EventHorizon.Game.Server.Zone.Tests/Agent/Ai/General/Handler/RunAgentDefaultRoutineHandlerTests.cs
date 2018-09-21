using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using System.Dynamic;
using EventHorizon.Game.Server.Zone.Core.Dynamic;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.General.Handler
{
    public class RunAgentDefaultRoutineHandlerTests
    {
        [Fact]
        public async Task TestHandle()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(123)).ReturnsAsync(new AgentEntity
            {
                Id = 123,
                Ai = new AgentAiState
                {
                    DefaultRoutine = AiRoutine.WANDER
                }
            });
            var handler = new RunAgentDefaultRoutineHandler(mediatorMock.Object, agentRepositoryMock.Object);
            // When
            await handler.Handle(new RunAgentDefaultRoutineEvent
            {
                AgentId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new StartAgentRoutineEvent
                {
                    AgentId = 123,
                    Routine = AiRoutine.WANDER
                }, It.IsAny<CancellationToken>())
            );
        }
        [Fact]
        public async Task TestHandle_ShouldNotStartRoutineWhenAgentNotFound()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(123)).ReturnsAsync(default(AgentEntity));
            var handler = new RunAgentDefaultRoutineHandler(mediatorMock.Object, agentRepositoryMock.Object);
            // When
            await handler.Handle(new RunAgentDefaultRoutineEvent
            {
                AgentId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                It.IsAny<StartAgentRoutineEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
        }
    }
}