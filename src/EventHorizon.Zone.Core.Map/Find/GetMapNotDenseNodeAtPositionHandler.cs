namespace EventHorizon.Zone.Core.Map.Find
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class GetMapNotDenseNodeAtPositionHandler
        : IRequestHandler<GetMapNotDenseNodeAtPosition, MapNode>
    {
        private readonly IMapGraph _map;

        public GetMapNotDenseNodeAtPositionHandler(
            IMapGraph map
        )
        {
            _map = map;
        }

        public Task<MapNode> Handle(
            GetMapNotDenseNodeAtPosition request,
            CancellationToken cancellationToken
        )
        {
            // TODO: Move this into a setting someplace, maybe IMapGraph related.
            var deltaToCheck = 10;
            var nodeAtPosition = _map.GetClosestNode(
                request.Position
            );

            var distanceToCheck = 0;
            while (InValidNode(nodeAtPosition))
            {
                distanceToCheck += 1;
                // Check for node in an ever incrementing distance from the
                //  node till one is found or the world is checked.
                var list = _map.GetClosestNodes(
                    request.Position,
                    distanceToCheck
                );
                var checkedNode = _map.GetClosestNodes(
                    request.Position,
                    distanceToCheck
                ).FirstOrDefault(
                    a => !a.Info.ContainsKey("dense")
                        || (int)a.Info["dense"] == 0
                );
                if (checkedNode.Info != null)
                {
                    nodeAtPosition = checkedNode;
                }
                if (distanceToCheck > deltaToCheck)
                {
                    break;
                }
            }

            if (InValidNode(nodeAtPosition))
            {
                // Invalid Node
                return default(MapNode).FromResult();
            }

            return Task.FromResult(
                nodeAtPosition
            );
        }

        private static bool InValidNode(
            MapNode nodeAtPosition
        )
        {
            return nodeAtPosition.Info.ContainsKey("dense")
                && (int)nodeAtPosition.Info["dense"] > 0;
        }
    }
}