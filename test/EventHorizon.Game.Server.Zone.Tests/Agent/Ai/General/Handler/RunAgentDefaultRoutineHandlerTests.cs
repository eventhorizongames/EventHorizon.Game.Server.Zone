using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using System.Dynamic;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Handlers;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Model.Entity;

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
            var agent = new AgentEntity
            {
                Id = 123,
                RawData = new Dictionary<string, object>()
                {
                    {
                        AgentRoutine.DEFAULT_ROUTINE_NAME,
                        AgentRoutine.WANDER
                    }
                }
            };
            agent.PopulateData<AgentRoutine>(AgentRoutine.DEFAULT_ROUTINE_NAME);

            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(123)).ReturnsAsync(agent);
            var handler = new RunAgentDefaultRoutineHandler(mediatorMock.Object, agentRepositoryMock.Object);

            // When
            await handler.Handle(new RunAgentDefaultRoutineEvent
            {
                EntityId = 123
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new StartAgentRoutineEvent
                {
                    EntityId = 123,
                    Routine = AgentRoutine.WANDER
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
                EntityId = 123
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                It.IsAny<StartAgentRoutineEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
        }
    }
}