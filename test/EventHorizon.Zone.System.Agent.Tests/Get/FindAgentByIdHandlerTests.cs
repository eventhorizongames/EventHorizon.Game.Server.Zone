using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Get;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Tests.Get
{
    public class FindAgentByIdHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnAgentFromRepositoryWhenAgentIdIsRequested()
        {
            // Given
            var agentId = "agent-id";
            var agent = new AgentEntity(
                null
            )
            {
                AgentId = agentId
            };
            var expected = agent;

            var agentRepositoryMock = new Mock<IAgentRepository>();

            agentRepositoryMock.Setup(
                mock => mock.FindByAgentId(
                    agentId
                )
            ).ReturnsAsync(
                agent
            );

            // When
            var handler = new FindAgentByIdHandler(
                agentRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new FindAgentByIdEvent(
                    agentId
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}