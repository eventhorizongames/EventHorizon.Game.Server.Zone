using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;
using EventHorizon.Zone.Core.Map.State;

namespace EventHorizon.Zone.Core.Map.Cost
{
    public struct ChangeEdgeCostForNodesAtPositionCommandHandler : IRequestHandler<ChangeEdgeCostForNodesAtPositionCommand, bool>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;
        public ChangeEdgeCostForNodesAtPositionCommandHandler(
            IMediator mediator,
            IMapGraph map
        )
        {
            _mediator = mediator;
            _map = map;
        }
        public async Task<bool> Handle(
            ChangeEdgeCostForNodesAtPositionCommand request,
            CancellationToken cancellationToken
        )
        {
            // Lookup node.
            var nodeList = await _mediator.Send(
                new GetMapNodesInDimensionsCommand(
                    request.Position,
                    request.BoundingBox
                )
            );
            foreach (var node in nodeList)
            {
                if (!node.Info.ContainsKey("dense"))
                {
                    node.Info.Add("dense", 0);
                }
                node.Info["dense"] = (int)node.Info["dense"] + 1;
                // Lookup edges at node.
                var edges = await _mediator.Send(
                    new GetMapEdgesOfNodeEvent
                    {
                        NodeIndex = node.Index
                    }
                );
                IList<MapEdge> updatedEdges = new List<MapEdge>();
                // Change Edges cost based on request.
                for (int i = 0; i < edges.Count(); i++)
                {
                    var edge = edges.First();
                    edge.Cost += request.Cost;
                    updatedEdges.Add(edge);
                }
                // Remove old edges
                foreach (var edge in edges)
                {
                    _map.RemoveEdge(edge);
                }
                // Save edges back into map
                foreach (var edge in updatedEdges)
                {
                    _map.AddEdge(edge);
                }
            }

            return true;
        }
    }
}