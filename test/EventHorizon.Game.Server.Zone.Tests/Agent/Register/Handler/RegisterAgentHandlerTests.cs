using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Agent.Register.Handler;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Register;
using System.Threading.Tasks;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Register;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Register.Handler
{
    public class RegisterAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnNotNullAgentWhenCalled()
        {
            // Given
            var entityId = 123;
            var treeId = "tree-id";
            var inputAgent = new AgentEntity();
            var inputAgentBehavior = new AgentBehavior();
            inputAgentBehavior.TreeId = treeId;

            var expectedAgent = new AgentEntity
            {
                Id = entityId,
                RawData = new Dictionary<string, object>()
                {
                    {
                        AgentBehavior.PROPERTY_NAME,
                        inputAgentBehavior
                    }
                }
            };
            expectedAgent.PopulateData<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );

            var expectedRegisterEntityEvent = new RegisterEntityEvent
            {
                Entity = inputAgent
            };
            var expectedRegisterActorWithBehaviorTreeUpdate = new RegisterActorWithBehaviorTreeUpdate(
                entityId,
                treeId
            );

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mediator => mediator.Send(
                    expectedRegisterEntityEvent,
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedAgent
            );
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(entityId)).ReturnsAsync(expectedAgent);

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

            mediatorMock.Verify(
                mediator => mediator.Send(
                    expectedRegisterEntityEvent,
                    CancellationToken.None
                )
            );
            agentRepositoryMock.Verify(
                agentRepository => agentRepository.FindById(
                    entityId
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    expectedRegisterActorWithBehaviorTreeUpdate,
                    CancellationToken.None
                )
            );
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
            agentRepositoryMock.Verify(a => a.Update(It.IsAny<AgentAction>(), It.IsAny<AgentEntity>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<RegisterActorWithBehaviorTreeUpdate>(), CancellationToken.None), Times.Never());
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
            mediatorMock.Setup(
                mediator => mediator.Send(
                    expectedRegisterEntityEvent,
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedAgent
            );
            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(
                agentRepository => agentRepository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                AgentEntity.CreateNotFound()
            );

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
            agentRepositoryMock.Verify(a => a.Update(It.IsAny<AgentAction>(), It.IsAny<AgentEntity>()), Times.Never());
            mediatorMock.Verify(mediator => mediator.Send(It.IsAny<RegisterActorWithBehaviorTreeUpdate>(), CancellationToken.None), Times.Never());
        }
    }
}