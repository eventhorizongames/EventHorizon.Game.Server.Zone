namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class GetMapNodeAtPositionHandler 
        : IRequestHandler<GetMapNodeAtPositionEvent, MapNode>
    {
        private readonly IMapGraph _map;

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
            return _map.GetClosestNode(
                request.Position
            ).FromResult();
        }
    }
}