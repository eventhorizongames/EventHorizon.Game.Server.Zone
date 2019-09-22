using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using MediatR;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Load.LoadAgentBehaviorSystem;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Load
{
    public class LoadAgentBehaviorSystemHandlerTests
    {
        [Fact]
        public async Task ShouldSendLoadActorBehaviorScriptsOnHandle()
        {
            // Given
            var expected = new LoadActorBehaviorScripts();

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
}