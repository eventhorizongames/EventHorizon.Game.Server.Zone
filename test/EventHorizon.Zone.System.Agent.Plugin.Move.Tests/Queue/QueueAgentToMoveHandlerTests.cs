namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Queue
{
    using global::System.Threading.Tasks;
    using global::System.Threading;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using Xunit;
    using Moq;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Move.Queue;

    public class QueueAgentToMoveHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldUpdateAgentPathAndAddToMoveRepository()
        {
            // Given
            var inputId = 123L;
            var expectedAgent = new AgentEntity(
                new Dictionary<string, object>()
            )
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
                new QueueAgentToMove(
                    inputId,
                    null,
                    Vector3.Zero
                ),
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