using Xunit;
using Moq;
using MediatR;
using EventHorizon.Zone.System.Agent.Move.Handler;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using System.Threading;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Events.Move;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class RegisterAgentMovePathHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldUpdateAgentPathAndAddToMoveRepository()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity
            {
                Id = inputId
            };

            var agentRepositoryMock = new Mock<IAgentRepository>();
            agentRepositoryMock.Setup(
                agentRepository => agentRepository.FindById(
                    inputId
                )
            ).ReturnsAsync(
                expectedAgent
            );
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var registerAgentMovePathHandler = new QueueAgentToMoveHandler(
                agentRepositoryMock.Object,
                moveAgentRepositoryMock.Object
            );
            await registerAgentMovePathHandler.Handle(
                new QueueAgentToMoveEvent
                {
                    EntityId = inputId
                },
                CancellationToken.None
            );

            // Then
            agentRepositoryMock.Verify(
                agentRepository => agentRepository.FindById(
                    inputId
                )
            );
            agentRepositoryMock.Verify(
                agentRepository => agentRepository.Update(
                    AgentAction.PATH,
                    expectedAgent
                )
            );
            moveAgentRepositoryMock.Verify(
                moveAgentRepository => moveAgentRepository.Register(
                    inputId
                )
            );
        }
    }
}