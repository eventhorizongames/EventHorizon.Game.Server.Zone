using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Events.Path;
using MediatR;
using EventHorizon.Zone.Core.Map.State;
using EventHorizon.Zone.Core.Map.Search;
using EventHorizon.Zone.Core.Model.Map;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class FindPathHandler : IRequestHandler<FindPathEvent, Queue<Vector3>>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;

        public FindPathHandler(
            IMediator mediator,
            IMapGraph map
        )
        {
            _mediator = mediator;
            _map = map;
        }
        
        public async Task<Queue<Vector3>> Handle(
            FindPathEvent request, 
            CancellationToken cancellationToken
        )
        {
            var fromMapNode = await _mediator.Send(
                new GetMapNotDenseNodeAtPosition
                {
                    Position = request.From,
                }
            );
            var toMapNode = await _mediator.Send(
                new GetMapNotDenseNodeAtPosition
                {
                    Position = request.To,
                }
            );

            return AStarSearch.CreatePath(
                _map,
                fromMapNode,
                toMapNode
            );
        }
    }
}