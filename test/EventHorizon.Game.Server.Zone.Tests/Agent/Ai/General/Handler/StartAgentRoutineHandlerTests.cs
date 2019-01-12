using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Handlers;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.General.Handler
{
    public class StartAgentRoutineHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldOnlyCallClearWhenAgentIdleRoutine()
        {
            // Given
            var inputId = 123L;
            var inputGetAgentEvent = new GetAgentEvent
            {
                EntityId = inputId
            };
            var expectedAgent = new AgentEntity
            {
                Id = inputId
            };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator.Send(inputGetAgentEvent, CancellationToken.None))
                .ReturnsAsync(expectedAgent);
            var handler = new StartAgentRoutineHandler(mediatorMock.Object);

            // When
            await handler.Handle(new StartAgentRoutineEvent
            {
                EntityId = inputId,
                Routine = AgentRoutine.IDLE
            }, CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(
                new ClearAgentRoutineEvent
                {
                    EntityId = inputId
                },
                It.IsAny<CancellationToken>()
            ));
            mediatorMock.Verify(mediator => mediator.Publish(
                It.IsAny<StartAgentMoveRoutineEvent>(),
                It.IsAny<CancellationToken>()
            ), Times.Never());
        }
    }
}