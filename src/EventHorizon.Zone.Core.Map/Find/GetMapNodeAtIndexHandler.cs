namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public class GetMapNodeAtIndexHandler
        : IRequestHandler<GetMapNodeAtIndexEvent, MapNode>
    {
        private readonly IMapGraph _map;

        public GetMapNodeAtIndexHandler(
            IMapGraph map
        )
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
