using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Events.Path;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Move
{
    public struct MoveAgentToPosition : IRequest
    {
        public long AgentId { get; set; }
        public Vector3 ToPosition { get; set; }
        public struct MoveAgentToPositionHandler : IRequestHandler<MoveAgentToPosition>
        {
            readonly IMediator _mediator;
            public MoveAgentToPositionHandler(
                IMediator mediator
            )
            {
                _mediator = mediator;
            }
            public async Task<Unit> Handle(
                MoveAgentToPosition request,
                CancellationToken cancellationToken
            )
            {
                // TODO: PerformanceTracker Add Performance Tracking Here to check the slowest spot
                // I have a hunch it is the Path From<>To lookup

                // Get entity position
                var agent = await _mediator.Send(new GetAgentEvent
                {
                    EntityId = request.AgentId,
                });
                if (!agent.IsFound())
                {
                    return Unit.Value;
                }
                // Get Path to node
                var path = await _mediator.Send(new FindPathEvent
                {
                    From = agent.Position.CurrentPosition,
                    To = request.ToPosition
                });
                // Register Path for Agent entity
                await _mediator.Publish(new RegisterAgentMovePathEvent
                {
                    EntityId = agent.Id,
                    Path = path
                });
                return Unit.Value;
            }
        }
    }
}