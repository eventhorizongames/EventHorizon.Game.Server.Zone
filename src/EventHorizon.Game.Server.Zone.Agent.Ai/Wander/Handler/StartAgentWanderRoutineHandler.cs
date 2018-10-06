using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using EventHorizon.Game.Server.Zone.Events.Map;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Wander.Handler
{
    public class StartAgentWanderRoutineHandler : INotificationHandler<StartAgentWanderRoutineEvent>
    {
        readonly IMediator _mediator;
        readonly IRandomNumberGenerator _random;
        public StartAgentWanderRoutineHandler(IMediator mediator, IRandomNumberGenerator random)
        {
            _mediator = mediator;
            _random = random;
        }
        public async Task Handle(StartAgentWanderRoutineEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _mediator.Send(new GetAgentEvent
            {
                AgentId = notification.AgentId
            });
            if (!agent.IsFound())
            {
                return;
            }
            // Get Map Nodes around Agent, within distance
            var mapNodes = await _mediator.Send(new GetMapNodesAroundPositionEvent
            {
                Position = agent.Position.CurrentPosition,
                Distance = agent.GetProperty<AgentWanderState>("Wander").LookDistance
            });
            if (mapNodes.Count == 0)
            {
                return;
            }
            var randomNodeIndex = _random.Next(0, mapNodes.Count);
            var node = mapNodes[randomNodeIndex];

            await _mediator.Publish(new StartAgentMoveRoutineEvent
            {
                AgentId = agent.Id,
                ToPosition = node.Position
            });
        }
    }
}