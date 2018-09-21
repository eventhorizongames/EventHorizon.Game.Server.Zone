using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class StopMoveRegisteredAgentsTimerHandlerTests
    {
        [Fact]
        public void TestHandle_ShouldCallStopOnInjectedMoveRegisteredAgentsTimerInstance()
        {
            // Given
            var moveRegisteredAgentsTimerMock = new Mock<IMoveRegisteredAgentsTimer>();

            // When
            var stopMoveRegisteredAgentsTimer = new StopMoveRegisteredAgentsTimerHandler(
                moveRegisteredAgentsTimerMock.Object
            );
            stopMoveRegisteredAgentsTimer.Handle(new StopMoveRegisteredAgentsTimerEvent(), CancellationToken.None);

            // Then
            moveRegisteredAgentsTimerMock.Verify(moveRegisteredAgentsTimer => moveRegisteredAgentsTimer.Stop());
        }
    }
}