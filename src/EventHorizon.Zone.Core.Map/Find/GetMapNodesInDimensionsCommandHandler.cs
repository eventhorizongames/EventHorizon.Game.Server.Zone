using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNodesInDimensionsCommandHandler
        : IRequestHandler<GetMapNodesInDimensionsCommand, IList<MapNode>>
    {
        readonly IMapGraph _map;
        public GetMapNodesInDimensionsCommandHandler(
            IMapGraph map
        )
        {
            _map = map;
        }
        public Task<IList<MapNode>> Handle(
            GetMapNodesInDimensionsCommand request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _map.GetClosestNodesInDimension(
                    request.Position,
                    request.Dimensions
                )
            );
        }
    }
}