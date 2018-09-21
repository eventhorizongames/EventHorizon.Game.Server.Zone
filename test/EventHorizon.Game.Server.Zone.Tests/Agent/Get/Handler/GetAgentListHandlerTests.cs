using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Get.Handler;
using EventHorizon.Game.Server.Zone.Agent.Get;
using System.Threading;
using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Get.Handler
{
    public class GetAgentListHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnExpectedAgentListFromPassedinRepository()
        {
            // Given
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var expectedAgent1 = new AgentEntity
            {
                Id = 123
            };
            var expectedAgent2 = new AgentEntity
            {
                Id = 321
            };
            var expectedAgentList = new List<AgentEntity>() {
                expectedAgent1,
                expectedAgent2
            };
            agentRepositoryMock.Setup(a => a.All()).ReturnsAsync(expectedAgentList);

            // When
            var getAgentHandler = new GetAgentListHandler(agentRepositoryMock.Object);
            var actual = await getAgentHandler.Handle(new GetAgentListEvent(), CancellationToken.None);
            // Then
            Assert.Collection(actual,
                agent => Assert.Equal(expectedAgent1, agent),
                agent => Assert.Equal(expectedAgent2, agent)
            );
        }
    }
}