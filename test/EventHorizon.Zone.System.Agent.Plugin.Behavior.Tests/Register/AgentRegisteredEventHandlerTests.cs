
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Register
{
    public class AgentRegisteredEventHandlerTests
    {
        [Fact]
        public async Task TestShouldSendRegisterUpdateEventWhenAgentIsFound()
        {
            // Given
            var agentId = "agent-id-001";
            var agentEntityId = 1L;
            var treeId = "tree-id";
            var expected = new RegisterActorWithBehaviorTreeUpdate(
                agentEntityId,
                treeId
            );
            var agentEntity = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = agentEntityId
            };
            agentEntity.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                new AgentBehavior
                {
                    TreeId = treeId
                }
            );

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new FindAgentByIdEvent(
                        agentId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentEntity
            );

            // When
            var handler = new AgentRegisteredEventHandler(
                mediatorMock.Object
            );

            await handler.Handle(
                new AgentRegisteredEvent(
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldNotSendRegisterUpdateEventWhenAgentIsNotFound()
        {
            // Given
            var agentId = "agent-id-001";
            var agentEntity = default(AgentEntity);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new FindAgentByIdEvent(
                        agentId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentEntity
            );

            // When
            var handler = new AgentRegisteredEventHandler(
                mediatorMock.Object
            );

            await handler.Handle(
                new AgentRegisteredEvent(
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RegisterActorWithBehaviorTreeUpdate>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}