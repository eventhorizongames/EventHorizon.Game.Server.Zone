using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Get;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Game.Server.Zone.Events.Path;
using static EventHorizon.Game.Server.Zone.Agent.Ai.Move.MoveAgentToPosition;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Performance;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.Move.Handler
{
    public class MoveAgentToPositionHandlerTest
    {
        [Fact]
        public async Task TestHandle_ShouldCallExpectedEvents()
        {
            // Given
            var inputId = 123L;
            var inputCurrentPosition = new Vector3(3);
            var inputToPosition = new Vector3(100);
            var inputGetAgentEvent = new GetAgentEvent
            {
                EntityId = inputId
            };
            var inputFindPathEvent = new FindPathEvent
            {
                From = inputCurrentPosition,
                To = inputToPosition
            };
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    CurrentPosition = inputCurrentPosition
                }
            };
            var expectedPath = new Queue<Vector3>();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mediator => mediator.Send(
                    inputGetAgentEvent, 
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedAgent
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    inputFindPathEvent, 
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedPath
            );
            // When            
            var moveAgentToPositionHandler = new MoveAgentToPositionHandler(
                mediatorMock.Object, 
                new Mock<IPerformanceTracker>().Object
            );
            await moveAgentToPositionHandler.Handle(
                new MoveAgentToPosition
                {
                    AgentId = inputId,
                    ToPosition = inputToPosition
                }, 
                CancellationToken.None
            );
            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new GetAgentEvent
                    {
                        EntityId = inputId
                    },
                    It.IsAny<CancellationToken>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new FindPathEvent
                    {
                        From = inputCurrentPosition,
                        To = inputToPosition
                    },
                    It.IsAny<CancellationToken>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    new QueueAgentToMoveEvent
                    {
                        EntityId = inputId,
                        Path = expectedPath
                    },
                    It.IsAny<CancellationToken>()
                )
            );
        }
    }
}