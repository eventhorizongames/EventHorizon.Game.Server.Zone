using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Get.Handler;
using EventHorizon.Game.Server.Zone.Agent.Get;
using System.Threading;
using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Get.Handler
{
    public class GetAgentHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldReturnExpectedAgentFromPassedinRepository()
        {
            // Given
            var inputId = 123;
            var agentRepositoryMock = new Mock<IAgentRepository>();
            var expectedAgent = new AgentEntity
            {
                Id = inputId
            };
            agentRepositoryMock.Setup(a => a.FindById(inputId)).ReturnsAsync(expectedAgent);

            // When
            var getAgentHandler = new GetAgentHandler(agentRepositoryMock.Object);
            var actual = await getAgentHandler.Handle(new GetAgentEvent
            {
                EntityId = inputId
            }, CancellationToken.None);
            // Then
            Assert.Equal(expectedAgent, actual);
        }
    }
}