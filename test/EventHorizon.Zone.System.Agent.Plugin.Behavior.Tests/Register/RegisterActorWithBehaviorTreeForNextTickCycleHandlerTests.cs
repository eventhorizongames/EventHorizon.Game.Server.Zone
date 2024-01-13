namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Register;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class RegisterActorWithBehaviorTreeForNextTickCycleHandlerTests
{
    [Fact]
    public async Task ShouldRegisterShapeIdAndActorIdWhenRequestIsHandled()
    {
        // Given
        var expectedShapeId = "shape-id";
        var expectedActorId = 1L;

        var actorBehaviorTickQueueMock = new Mock<ActorBehaviorTickQueue>();

        // When
        var handler = new RegisterActorWithBehaviorTreeForNextTickCycleHandler(
            actorBehaviorTickQueueMock.Object
        );

        await handler.Handle(
            new RegisterActorWithBehaviorTreeForNextTickCycle(
                expectedShapeId,
                expectedActorId
            ),
            CancellationToken.None
        );

        // Then
        actorBehaviorTickQueueMock.Verify(
            mock => mock.Register(
                expectedShapeId,
                expectedActorId
            )
        );
    }
}
