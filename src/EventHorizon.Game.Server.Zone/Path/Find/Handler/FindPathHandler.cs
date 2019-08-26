using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.Events.Path;
using EventHorizon.Game.Server.Zone.Path.Search;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Path.Find.Handler
{
    public struct FindPathHandler : IRequestHandler<FindPathEvent, Queue<Vector3>>
    {
        readonly IMediator _mediator;
        readonly IServerState _serverState;
        public FindPathHandler(
            IMediator mediator,
            IServerState serverState
        )
        {
            _mediator = mediator;
            _serverState = serverState;
        }
        public async Task<Queue<Vector3>> Handle(FindPathEvent request, CancellationToken cancellationToken)
        {
            var fromMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = request.From,
            });
            var toMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = request.To,
            });

            return AStarSearch.CreatePath(
                await _serverState.Map(),
                fromMapNode,
                toMapNode
            );
        }
    }
}