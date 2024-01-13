namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Register;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;

using global::System.Collections.Concurrent;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class AgentRegisteredEventHandlerTests
{
    [Fact]
    public async Task TestShouldSendRegisterUpdateEventWhenAgentIsFound()
    {
        // Given
        var agentId = "agent-id-001";
        var agentEntityId = 1L;
        var treeId = "tree-id";
        var agentEntity = new AgentEntity(
            new ConcurrentDictionary<string, object>()
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
        var expected = new ChangeActorBehaviorTreeCommand(
            agentEntity,
            treeId
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
                It.IsAny<RegisterActorWithBehaviorTreeForNextTickCycle>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }
}
