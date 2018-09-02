using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move.Handler;
using EventHorizon.Game.Server.Zone.Path.Find;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.Move.Handler
{
    public class StartAgentMoveRoutineHandlerTest
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
                AgentId = inputId
            };
            var inputFindPathEvent = new FindPathEvent
            {
                From = inputCurrentPosition,
                To = inputToPosition
            };
            var expectedAgent = new AgentEntity
            {
                Id = inputId,
                Position = new Core.Model.PositionState
                {
                    CurrentPosition = inputCurrentPosition
                }
            };
            var expectedPath = new Queue<Vector3>();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(inputGetAgentEvent, CancellationToken.None))
                .ReturnsAsync(expectedAgent);
            mediatorMock.Setup(mediator => mediator.Send(inputFindPathEvent, CancellationToken.None))
                .ReturnsAsync(expectedPath);
            var handler = new StartAgentMoveRoutineHandler(mediatorMock.Object);
            // When
            await handler.Handle(new StartAgentMoveRoutineEvent
            {
                AgentId = inputId,
                ToPosition = inputToPosition
            }, CancellationToken.None);
            // Then
            mediatorMock.Verify(mediator => mediator.Send(
                new GetAgentEvent
                {
                    AgentId = inputId
                },
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Send(
                new FindPathEvent
                {
                    From = inputCurrentPosition,
                    To = inputToPosition
                },
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Publish(
                new RegisterAgentMovePathEvent
                {
                    AgentId = inputId,
                    Path = expectedPath
                },
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Publish(
                new SetAgentRoutineEvent
                {
                    AgentId = inputId,
                    Routine = AiRoutine.MOVE
                },
                It.IsAny<CancellationToken>()
            ));
        }
    }
}