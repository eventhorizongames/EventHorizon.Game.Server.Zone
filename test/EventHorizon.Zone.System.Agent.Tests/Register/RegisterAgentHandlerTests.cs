using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Register.Handler;
using MediatR;
using Moq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Register.Handler
{
    public class RegisterAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnNotNullAgentWhenCalled()
        {
            // Given
            var entityId = 123;
            var agentId = "agent-id";
            var inputAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                AgentId = agentId
            };
            var expectedAgentRegisterEvent = new AgentRegisteredEvent(
                agentId
            );
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = entityId
            };

            var expectedRegisterEntityEvent = new RegisterEntityEvent
            {
                Entity = inputAgent
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
                    entityId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var registerAgentHandler = new RegisterAgentHandler(
                mediatorMock.Object,
                agentRepositoryMock.Object
            );
            var actual = await registerAgentHandler.Handle(
                new RegisterAgentEvent
                {
                    Agent = inputAgent
                },
                CancellationToken.None
            );

            // Then
            Assert.True(
                actual.IsFound()
            );
            Assert.Equal(
                expectedAgent,
                actual
            );

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
        }
        [Fact]
        public async Task TestHandle_ShouldReturnNotFoundAgentWhenRegisterEntityEventFailed()
        {
            // Given
            var inputAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RegisterEntityEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                default(AgentEntity)
            );
            var agentRepositoryMock = new Mock<IAgentRepository>();

            // When
            var registerAgentHandler = new RegisterAgentHandler(
                mediatorMock.Object,
                agentRepositoryMock.Object
            );
            var actual = await registerAgentHandler.Handle(
                new RegisterAgentEvent
                {
                    Agent = inputAgent
                },
                CancellationToken.None
            );

            // Then
            Assert.False(
                actual.IsFound()
            );
            agentRepositoryMock.Verify(
                a => a.Update(
                    It.IsAny<AgentAction>(),
                    It.IsAny<AgentEntity>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<AgentRegisteredEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
        [Fact]
        public async Task TestHandle_ShouldReturnNotFoundAgentWhenAgentEntityIsNotFoundInRepository()
        {
            // Given
            var inputId = 123;
            var inputAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            );

            var expectedRegisterEntityEvent = new RegisterEntityEvent
            {
                Entity = inputAgent
            };
            var expectedAgent = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
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
            var actual = await registerAgentHandler.Handle(
                new RegisterAgentEvent
                {
                    Agent = inputAgent
                },
                CancellationToken.None
            );

            // Then
            Assert.False(
                actual.IsFound()
            );
            agentRepositoryMock.Verify(
                a => a.Update(
                    It.IsAny<AgentAction>(),
                    It.IsAny<AgentEntity>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<AgentRegisteredEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}