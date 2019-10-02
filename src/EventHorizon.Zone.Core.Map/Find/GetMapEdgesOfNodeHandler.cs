using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapEdgesOfNodeHandler : IRequestHandler<GetMapEdgesOfNodeEvent, IEnumerable<MapEdge>>
    {
        readonly IMapGraph _map;
        public GetMapEdgesOfNodeHandler(
            IMapGraph map
        )
        {
            _map = map;
        }

        public Task<IEnumerable<MapEdge>> Handle(
            GetMapEdgesOfNodeEvent request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _map.GetEdgesOfNode(
                    request.NodeIndex
                )
            );
        }
    }
}