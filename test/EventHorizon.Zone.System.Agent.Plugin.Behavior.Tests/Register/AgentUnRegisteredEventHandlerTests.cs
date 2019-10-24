
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Register
{
    public class AgentUnRegisteredEventHandlerTests
    {
        [Fact]
        public async Task TestShouldSendUnRegisterUpdateEventWhenAgentIsFound()
        {
            // Given
            var agentEntityId = 1L;
            var agentId = "agent-id-001";
            var treeId = "tree-id";
            var expected = new UnRegisterActorWithBehaviorTreeUpdate(
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

            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();

            // When
            var handler = new AgentUnRegisteredEventHandler(
                actorBehaviorTreeRepositoryMock.Object
            );

            await handler.Handle(
                new AgentUnRegisteredEvent(
                    agentEntityId,
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                mock => mock.UnRegisterActor(
                    agentEntityId
                )
            );
        }
    }
}