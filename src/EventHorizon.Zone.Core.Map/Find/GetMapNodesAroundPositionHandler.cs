namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class GetMapNodesAroundPositionHandler
        : IRequestHandler<GetMapNodesAroundPositionEvent, IList<MapNode>>
    {
        private readonly IMapGraph _map;

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
            return _map.GetClosestNodes(
                request.Position,
                request.Distance
            ).FromResult();
        }
    }
}