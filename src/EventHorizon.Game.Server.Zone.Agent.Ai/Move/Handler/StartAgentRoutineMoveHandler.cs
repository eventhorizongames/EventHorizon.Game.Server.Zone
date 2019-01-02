

using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Game.Server.Zone.Agent.Ai.Wander;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
{
    public class StartAgentRoutineMoveHandler : INotificationHandler<StartAgentRoutineEvent>
    {
        readonly IMediator _mediator;
        public StartAgentRoutineMoveHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(StartAgentRoutineEvent request, CancellationToken cancellationToken)
        {
            // Get the Agent Entity
            var agent = await _mediator.Send(new GetAgentEvent
            {
                EntityId = request.EntityId,
            });
            if (!agent.IsFound() || !AgentRoutine.MOVE.Equals(request.Routine))
            {
                return;
            }
            // Clear any already in process Routines
            await _mediator.Publish(new ClearAgentRoutineEvent
            {
                EntityId = request.EntityId
            });
            // Start Routine to Move the Agent to the MoveToPosition
            await _mediator.Publish(new StartAgentMoveRoutineEvent
            {
                EntityId = agent.Id,
                ToPosition = agent.Position.MoveToPosition
            });
        }
    }
}