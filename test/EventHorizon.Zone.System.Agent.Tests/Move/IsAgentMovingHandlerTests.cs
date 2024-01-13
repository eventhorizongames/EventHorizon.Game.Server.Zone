namespace EventHorizon.Zone.System.Agent.Tests.Move;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.Path;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Move;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class IsAgentMovingHandlerTests
{
    [Fact]
    public async Task TestShouldReturnTrueWhenAgentHasItemsInTherePath()
    {
        // Given
        var entityId = 2L;
        var path = new Queue<Vector3>(
            new List<Vector3> {
                Vector3.Zero
            }
        );
        var rawData = new ConcurrentDictionary<string, object>();
        rawData[PathState.PROPERTY_NAME] = new PathState()
            .SetPath(path);
        var agent = new AgentEntity(
            rawData
        );
        agent.PopulateData<PathState>(
            PathState.PROPERTY_NAME
        );

        var agentRepositoryMock = new Mock<IAgentRepository>();

        agentRepositoryMock.Setup(
            mock => mock.FindById(
                entityId
            )
        ).ReturnsAsync(
            agent
        );

        // When
        var handler = new IsAgentMovingHandler(
            agentRepositoryMock.Object
        );
        var actual = await handler.Handle(
            new IsAgentMoving(
                entityId
            ),
            CancellationToken.None
        );

        // Then
        Assert.True(
            actual
        );
    }

    [Fact]
    public async Task TestShouldReturnFalseWhenAgentPathIsNull()
    {
        // Given
        var entityId = 2L;
        var rawData = new ConcurrentDictionary<string, object>();
        rawData[PathState.PROPERTY_NAME] = new PathState()
            .SetPath(null);
        var agent = new AgentEntity(
            rawData
        );
        agent.PopulateData<PathState>(
            PathState.PROPERTY_NAME
        );

        var agentRepositoryMock = new Mock<IAgentRepository>();

        agentRepositoryMock.Setup(
            mock => mock.FindById(
                entityId
            )
        ).ReturnsAsync(
            agent
        );

        // When
        var handler = new IsAgentMovingHandler(
            agentRepositoryMock.Object
        );
        var actual = await handler.Handle(
            new IsAgentMoving(
                entityId
            ),
            CancellationToken.None
        );

        // Then
        Assert.False(
            actual
        );
    }

    [Fact]
    public async Task TestShouldReturnFalseWhenAgentPathIsEmpty()
    {
        // Given
        var entityId = 2L;
        var rawData = new ConcurrentDictionary<string, object>();
        rawData[PathState.PROPERTY_NAME] = new PathState()
            .SetPath(
                new Queue<Vector3>()
            );
        var agent = new AgentEntity(
            rawData
        );
        agent.PopulateData<PathState>(
            PathState.PROPERTY_NAME
        );

        var agentRepositoryMock = new Mock<IAgentRepository>();

        agentRepositoryMock.Setup(
            mock => mock.FindById(
                entityId
            )
        ).ReturnsAsync(
            agent
        );

        // When
        var handler = new IsAgentMovingHandler(
            agentRepositoryMock.Object
        );
        var actual = await handler.Handle(
            new IsAgentMoving(
                entityId
            ),
            CancellationToken.None
        );

        // Then
        Assert.False(
            actual
        );
    }
}
