using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Register.Handler;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Register;
using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Register.Handler
{
    public class RegisterAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnNotNullAgentWhenCalled()
        {
            // Given
            var agentId = 123;
            var inputRoutine = AiRoutine.IDLE;
            var inputAgent = new AgentEntity();

            var expectedAgent = new AgentEntity
            {
                Id = agentId,
                Ai = new AgentAiDetails
                {
                    DefaultRoutine = inputRoutine
                }
            };
            var expectedRegisterEntityEvent = new RegisterEntityEvent
            {
                Entity = inputAgent
            };
            var expectedStartAgentRoutineEvent = new StartAgentRoutineEvent
            {
                Routine = inputRoutine,
                AgentId = agentId
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(expectedRegisterEntityEvent, CancellationToken.None)).ReturnsAsync(expectedAgent);
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(agentId)).ReturnsAsync(expectedAgent);

            // When
            var registerAgentHandler = new RegisterAgentHandler(
                mediatorMock.Object,
                agentRepositoryMock.Object
            );
            var actual = await registerAgentHandler.Handle(new RegisterAgentEvent
            {
                Agent = inputAgent
            }, CancellationToken.None);

            // Then
            Assert.True(actual.IsFound());
            Assert.Equal(expectedAgent, actual);

            mediatorMock.Verify(mediator => mediator.Send(expectedRegisterEntityEvent, CancellationToken.None));
            agentRepositoryMock.Verify(agentRepository => agentRepository.FindById(agentId));
            mediatorMock.Verify(mediator => mediator.Send(expectedStartAgentRoutineEvent, CancellationToken.None));
        }
        [Fact]
        public async Task TestHandle_ShouldReturnNotFoundAgentWhenRegisterEntityEventFailed()
        {
            // Given
            var inputAgent = new AgentEntity();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(It.IsAny<RegisterEntityEvent>(), CancellationToken.None)).ReturnsAsync(default(AgentEntity));
            var agentRepositoryMock = new Mock<IAgentRepository>();

            // When
            var registerAgentHandler = new RegisterAgentHandler(
                mediatorMock.Object,
                agentRepositoryMock.Object
            );
            var actual = await registerAgentHandler.Handle(new RegisterAgentEvent
            {
                Agent = inputAgent
            }, CancellationToken.None);

            // Then
            Assert.False(actual.IsFound());
            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<StartAgentRoutineEvent>(), CancellationToken.None), Times.Never());
        }
        [Fact]
        public async Task TestHandle_ShouldReturnNotFoundAgentWhenAgentEntityIsNotFoundInRepository()
        {
            // Given
            var inputId = 123;
            var inputAgent = new AgentEntity();

            var expectedRegisterEntityEvent = new RegisterEntityEvent
            {
                Entity = inputAgent
            };
            var expectedAgent = new AgentEntity
            {
                Id = inputId
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(expectedRegisterEntityEvent, CancellationToken.None)).ReturnsAsync(expectedAgent);
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(inputId)).ReturnsAsync(AgentEntity.CreateNotFound());

            // When
            var registerAgentHandler = new RegisterAgentHandler(
                mediatorMock.Object,
                agentRepositoryMock.Object
            );
            var actual = await registerAgentHandler.Handle(new RegisterAgentEvent
            {
                Agent = inputAgent
            }, CancellationToken.None);

            // Then
            Assert.False(actual.IsFound());
            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<StartAgentRoutineEvent>(), CancellationToken.None), Times.Never());
        }
    }
}