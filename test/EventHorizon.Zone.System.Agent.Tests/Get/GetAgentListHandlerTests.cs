using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Get;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using Moq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Get.Handler
{
    public class GetAgentListHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnExpectedAgentListFromPassedinRepository()
        {
            // Given
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var expectedAgent1 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = 123
            };
            var expectedAgent2 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = 321
            };
            var expectedAgentList = new List<AgentEntity>() {
                expectedAgent1,
                expectedAgent2
            };
            agentRepositoryMock.Setup(
                a => a.All()
            ).ReturnsAsync(
                expectedAgentList
            );

            // When
            var getAgentHandler = new GetAgentListHandler(
                agentRepositoryMock.Object
            );
            var actual = await getAgentHandler.Handle(
                new GetAgentListEvent(),
                CancellationToken.None
            );
            // Then
            Assert.Collection(actual,
                agent => Assert.Equal(
                    expectedAgent1,
                    agent
                ),
                agent => Assert.Equal(
                    expectedAgent2,
                    agent
                )
            );
        }
    }
}