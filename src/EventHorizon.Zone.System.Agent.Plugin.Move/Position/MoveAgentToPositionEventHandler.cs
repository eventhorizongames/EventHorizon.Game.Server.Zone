using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Path;
using EventHorizon.Performance;
using MediatR;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

namespace EventHorizon.Zone.System.Agent.Plugin.Move.Position
{
    public struct MoveAgentToPositionEventHandler : IRequestHandler<MoveAgentToPositionEvent>
    {
        readonly IMediator _mediator;
        readonly IPerformanceTracker _performanceTracker;
        public MoveAgentToPositionEventHandler(
            IMediator mediator,
            IPerformanceTracker performanceTracker
        )
        {
            _mediator = mediator;
            _performanceTracker = performanceTracker;
        }
        public async Task<Unit> Handle(
            MoveAgentToPositionEvent request,
            CancellationToken cancellationToken
        )
        {
            // Get entity position
            var agent = await _mediator.Send(new GetAgentEvent
            {
                EntityId = request.AgentId,
            });
            if (!agent.IsFound())
            {
                return Unit.Value;
            }
            Queue<Vector3> path;
            // Get Path to node
            path = await _mediator.Send(new FindPathEvent
            {
                From = agent.Position.CurrentPosition,
                To = request.ToPosition
            });

            // Register Path for Agent entity
            await _mediator.Publish(new QueueAgentToMoveEvent
            {
                EntityId = agent.Id,
                Path = path
            });
            return Unit.Value;
        }
    }
}