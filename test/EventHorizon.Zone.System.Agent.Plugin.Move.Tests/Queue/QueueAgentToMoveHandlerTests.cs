namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests.Queue;

using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Move.Queue;

using global::System.Collections.Concurrent;
using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class QueueAgentToMoveHandlerTests
{
    [Fact]
    public async Task TestHandle_ShouldUpdateAgentPathAndAddToMoveRepository()
    {
        // Given
        var inputId = 123L;
        var expectedAgent = new AgentEntity(
            new ConcurrentDictionary<string, object>()
        )
        {
            Id = inputId
        };

        var mediatorMock = new Mock<IMediator>();
        var agentRepositoryMock = new Mock<IAgentRepository>();
        var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

        agentRepositoryMock.Setup(
            mock => mock.FindById(
                inputId
            )
        ).ReturnsAsync(
            expectedAgent
        );

        // When
        var registerAgentMovePathHandler = new QueueAgentToMoveHandler(
            mediatorMock.Object,
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
            mock => mock.FindById(
                inputId
            )
        );
        mediatorMock.Verify(
            mock => mock.Send(
                new UpdateEntityCommand(
                    AgentAction.PATH,
                    expectedAgent
                ),
                CancellationToken.None
            )
        );
        moveAgentRepositoryMock.Verify(
            mock => mock.Register(
                inputId
            )
        );
    }
}
