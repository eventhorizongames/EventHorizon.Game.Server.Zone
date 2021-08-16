namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public class GetMapNodesInDimensionsCommandHandler
        : IRequestHandler<GetMapNodesInDimensionsCommand, IList<MapNode>>
    {
        private readonly IMapGraph _map;

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
            return _map.GetClosestNodesInDimension(
                request.Position,
                request.Dimensions
            ).FromResult();
        }
    }
}
