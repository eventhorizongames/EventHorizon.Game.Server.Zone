namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class UpdateDensityAndCostDetailsForNodeHandler : IRequestHandler<UpdateDensityAndCostDetailsForNode>
    {
        private readonly IMediator _mediator;
        private readonly IMapGraph _graph;

        public UpdateDensityAndCostDetailsForNodeHandler(
            IMediator mediator,
            IMapGraph graph
        )
        {
            _mediator = mediator;
            _graph = graph;
        }

        public async Task<Unit> Handle(
            UpdateDensityAndCostDetailsForNode request,
            CancellationToken cancellationToken
        )
        {
            var node = request.Node;
            var dense = request.Dense;
            var cost = request.Cost;

            if (!node.Info.ContainsKey("dense"))
            {
                node.Info.Add("dense", 0);
            }
            node.Info["dense"] = (int)node.Info["dense"] + dense;
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
                edge.Cost += cost;
                updatedEdges.Add(edge);
            }
            // Remove old edges
            foreach (var edge in edges)
            {
                // TODO: Move this to an event
                _graph.RemoveEdge(edge);
            }
            // Save edges back into map
            foreach (var edge in updatedEdges)
            {
                // TODO: Move this to an event
                _graph.AddEdge(edge);
            }
            return Unit.Value;
        }
    }
}