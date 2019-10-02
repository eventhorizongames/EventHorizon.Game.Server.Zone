using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNodesAroundPositionHandler
        : IRequestHandler<GetMapNodesAroundPositionEvent, IList<MapNode>>
    {
        readonly IMapGraph _map;
        public GetMapNodesAroundPositionHandler(
            IMapGraph map
        )
        {
            _map = map;
        }
        public Task<IList<MapNode>> Handle(
            GetMapNodesAroundPositionEvent request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _map.GetClosestNodes(
                    request.Position,
                    request.Distance
                )
            );
        }
    }
}