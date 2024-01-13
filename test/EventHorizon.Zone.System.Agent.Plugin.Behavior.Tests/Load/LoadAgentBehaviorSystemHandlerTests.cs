namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class LoadAgentBehaviorSystemHandlerTests
{
    [Fact]
    public async Task ShouldSendLoadActorBehaviorTreeShapesOnHandle()
    {
        // Given
        var expected = new LoadActorBehaviorTreeShapes();

        var mediatorMock = new Mock<IMediator>();

        // When
        var loadAgentBehaviorSystemHandler = new LoadAgentBehaviorSystemHandler(
            mediatorMock.Object
        );

        await loadAgentBehaviorSystemHandler.Handle(
            new LoadAgentBehaviorSystem(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mediator => mediator.Send(
                expected,
                CancellationToken.None
            )
        );
    }
}
