using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Events;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class ClearAgentRoutineHandlerTests
    {
        [Fact]
        public void Test()
        {
            // Given
            var inputId = 123;
            var moveAgentRepositoryMock = new Mock<IMoveAgentRepository>();

            // When
            var clearAgentRoutineHandler = new ClearAgentRoutineHandler(moveAgentRepositoryMock.Object);
            clearAgentRoutineHandler.Handle(new ClearAgentRoutineEvent
            {
                EntityId = inputId
            }, CancellationToken.None);
            // Then
            moveAgentRepositoryMock.Verify(repository => repository.Remove(inputId));
        }
    }
}