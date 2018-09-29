
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Flee;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
{
    public class StartAgentRoutineFleeHandler : INotificationHandler<StartAgentRoutineEvent>
    {
        readonly IMediator _mediator;
        public StartAgentRoutineFleeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(StartAgentRoutineEvent request, CancellationToken cancellationToken)
        {
            // Get the Agent Entity
            var agent = await _mediator.Send(new GetAgentEvent
            {
                AgentId = request.AgentId,
            });
            if (!agent.IsFound() || !AgentRoutine.FLEE.Equals(request.Routine))
            {
                return;
            }
            // Clear any already in process Routines
            await _mediator.Publish(new ClearAgentRoutineEvent
            {
                AgentId = request.AgentId
            });
            // Start Flee Routine.
            await _mediator.Publish(new StartAgentFleeRoutineEvent
            {
                AgentId = agent.Id
            });
        }
    }
}