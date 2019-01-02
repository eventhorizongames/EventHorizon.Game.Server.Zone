using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class RegisterAgentMovePathHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldUpdateAgentPathAndAddToMoveRepository()
        {
            // Given
            var inputId = 123;
            var expectedAgent = new AgentEntity
            {
                Id = inputId
            };

            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(agentRepository => agentRepository.FindById(inputId)).ReturnsAsync(expectedAgent);
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var registerAgentMovePathHandler = new RegisterAgentMovePathHandler(
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await registerAgentMovePathHandler.Handle(new RegisterAgentMovePathEvent
            {
                EntityId = inputId
            }, CancellationToken.None);

            // Then
            agentRepositoryMock.Verify(agentRepository => agentRepository.FindById(inputId));
            agentRepositoryMock.Verify(agentRepository => agentRepository.Update(AgentAction.PATH, expectedAgent));
            moveAgentRepositoryMock.Verify(moveAgentRepository => moveAgentRepository.Add(inputId));
        }
    }
}