using Xunit;
using Moq;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Get;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Events.Get;
using System.Collections.Generic;

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
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
            {
                Id = inputId
            };
            agentRepositoryMock.Setup(
                a => a.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );

            // When
            var getAgentHandler = new GetAgentHandler(
                agentRepositoryMock.Object
            );
            var actual = await getAgentHandler.Handle(
                new GetAgentEvent
                {
                    EntityId = inputId
                }, 
                CancellationToken.None
            );
            // Then
            Assert.Equal(
                expectedAgent, 
                actual
            );
        }
    }
}