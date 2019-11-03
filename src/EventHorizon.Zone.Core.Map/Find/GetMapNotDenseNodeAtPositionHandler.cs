using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNotDenseNodeAtPositionHandler : IRequestHandler<GetMapNotDenseNodeAtPosition, MapNode>
    {
        readonly IMapGraph _map;

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
            var nodeAtPosition = _map.GetClosestNode(
                request.Position
            );

            var distanceToCheck = 0;
            while (
                nodeAtPosition.Info.ContainsKey("dense") 
                && (int)nodeAtPosition.Info["dense"] > 0
            )
            {
                distanceToCheck += 1;
                // Check for node in an ever incrementing distance from the node till one is found or the world is checked.
                var checkedNode = _map.GetClosestNodes(
                    request.Position,
                    distanceToCheck
                ).FirstOrDefault(
                    a => !a.Info.ContainsKey("dense") 
                        || (int)nodeAtPosition.Info["dense"] == 0
                );
                if (checkedNode.Info != null)
                {
                    nodeAtPosition = checkedNode;
                }
            }

            return Task.FromResult(
                nodeAtPosition
            );
        }
    }
}