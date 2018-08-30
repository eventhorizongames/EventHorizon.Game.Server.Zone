using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Game.Server.Zone.Agent.Ai.Wander;
using EventHorizon.Game.Server.Zone.Agent.Get;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler
{
    public class StartAgentRoutineHandler : IRequestHandler<StartAgentRoutineEvent>
    {
        readonly IMediator _mediator;
        public StartAgentRoutineHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Unit> Handle(StartAgentRoutineEvent request, CancellationToken cancellationToken)
        {
            // Get the Agent Entity
            var agent = await _mediator.Send(new GetAgentEvent
            {
                AgentId = request.AgentId,
            });
            if (!agent.IsFound())
            {
                return Unit.Value;
            }
            // Clear any already in process Routines
            await _mediator.Publish(new ClearAgentRoutineEvent
            {
                AgentId = request.AgentId
            });

            switch (request.Routine)
            {
                case AiRoutine.MOVE:
                    // Start Routine to Move the Agent to the MoveToPosition
                    await _mediator.Publish(new StartAgentMoveRoutineEvent
                    {
                        AgentId = agent.Id,
                        ToPosition = agent.Position.MoveToPosition
                    });
                    break;
                case AiRoutine.WANDER:
                    // Start Routine to have the Agent Wander around the area.
                    await _mediator.Publish(new StartAgentWanderRoutineEvent
                    {
                        AgentId = agent.Id,
                        Wander = agent.TypedData.Wander
                    });
                    break;
                case AiRoutine.IDLE:
                default:
                    break;
            }

            return Unit.Value;
        }
    }
}