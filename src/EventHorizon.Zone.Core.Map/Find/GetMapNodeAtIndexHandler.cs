using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNodeAtIndexHandler : IRequestHandler<GetMapNodeAtIndexEvent, MapNode>
    {
        readonly IMapGraph _map;
        public GetMapNodeAtIndexHandler(
            IMapGraph map)
        {
            _map = map;
        }
        public Task<MapNode> Handle(
            GetMapNodeAtIndexEvent request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _map.GetNode(
                    request.NodeIndex
                )
            );
        }
    }
}