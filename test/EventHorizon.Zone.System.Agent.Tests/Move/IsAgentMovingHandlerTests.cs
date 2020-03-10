using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.Path;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Move;
using Moq;
using Xunit;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Agent.Tests.Move
{
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
            var agent = new AgentEntity(
                new Dictionary<string, object>
                {
                    { PathState.PROPERTY_NAME, new PathState{ Path = path } }
                }
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
            var agent = new AgentEntity(
                new Dictionary<string, object>
                {
                    {
                        PathState.PROPERTY_NAME,
                        new PathState
                        {
                            Path = null
                        }
                    }
                }
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
            var agent = new AgentEntity(
                new Dictionary<string, object>
                {
                    {
                        PathState.PROPERTY_NAME,
                        new PathState
                        {
                            Path = new Queue<Vector3>()
                        }
                    }
                }
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
}