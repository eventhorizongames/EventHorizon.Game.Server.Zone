using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class StartMoveRegisteredAgentsTimerHandlerTests
    {
        [Fact]
        public void TestHandle_ShouldCallStartOnInjectedMoveRegisteredAgentsTimerInstance()
        {
            // Given
            var moveRegisteredAgentsTimerMock = new Mock<IMoveRegisteredAgentsTimer>();

            // When
            var startMoveRegisteredAgentsTimer = new StartMoveRegisteredAgentsTimerHandler(
                moveRegisteredAgentsTimerMock.Object
            );
            startMoveRegisteredAgentsTimer.Handle(new StartMoveRegisteredAgentsTimerEvent(), CancellationToken.None);

            // Then
            moveRegisteredAgentsTimerMock.Verify(moveRegisteredAgentsTimer => moveRegisteredAgentsTimer.Start());
        }
    }
}