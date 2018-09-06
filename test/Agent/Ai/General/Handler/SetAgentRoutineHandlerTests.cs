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
    public class SetAgentRoutineHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldUpdateAgent()
        {
            // Given
            var inputId = 123L;
            var inputAgent = new AgentEntity
            {
                Id = inputId
            };
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(inputId))
                .ReturnsAsync(inputAgent);
                
            // When
            var handler = new SetAgentRoutineHandler(agentRepositoryMock.Object);
            await handler.Handle(new SetAgentRoutineEvent
            {
                AgentId = inputId,
                Routine = AiRoutine.SCRIPT
            }, CancellationToken.None);

            // Then
            agentRepositoryMock.Verify(repository => repository.Update(
                AgentAction.ROUTINE,
                It.IsAny<AgentEntity>()
            ));
        }
        [Fact]
        public async Task TestHandle_ShouldNotStartRoutineWhenAgentNotFound()
        {
            // Given
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(123))
                .ReturnsAsync(default(AgentEntity));

            // When
            var handler = new SetAgentRoutineHandler(agentRepositoryMock.Object);
            await handler.Handle(new SetAgentRoutineEvent
            {
                AgentId = 123
            }, CancellationToken.None);

            // Then
            agentRepositoryMock.Verify(repository => repository.Update(
                It.IsAny<AgentAction>(),
                It.IsAny<AgentEntity>()
            ), Times.Never());
        }
    }
}