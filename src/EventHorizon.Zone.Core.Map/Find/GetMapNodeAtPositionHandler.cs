using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNodeAtPositionHandler : IRequestHandler<GetMapNodeAtPositionEvent, MapNode>
    {
        readonly IMapGraph _map;
        public GetMapNodeAtPositionHandler(
            IMapGraph map
        )
        {
            _map = map;
        }
        public Task<MapNode> Handle(
            GetMapNodeAtPositionEvent request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _map.GetClosestNode(
                    request.Position
                )
            );
        }
    }
}