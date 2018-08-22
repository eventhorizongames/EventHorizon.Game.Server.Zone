using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Path.Find;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Move.Handler
{
    public class StartAgentMoveRoutineHandler : INotificationHandler<StartAgentMoveRoutineEvent>
    {
        readonly IMediator _mediator;
        public StartAgentMoveRoutineHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(StartAgentMoveRoutineEvent notification, CancellationToken cancellationToken)
        {
            // Get entity position
            var agent = await _mediator.Send(new GetAgentEvent
            {
                AgentId = notification.AgentId,
            });
            // Get Path to node
            var path = await _mediator.Send(new FindPathEvent
            {
                From = agent.Position.CurrentPosition,
                To = notification.ToPosition
            });
            // Register Path for Agent entity
            await _mediator.Publish(new RegisterAgentMovePathEvent
            {
                AgentId = agent.Id,
                Path = path
            });
            // Set Agents Routine
            await _mediator.Publish(new SetAgentRoutineEvent
            {
                AgentId = agent.Id,
                Routine = AiRoutine.MOVE
            });
        }
    }
}