using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Cost
{
    public struct ChangeEdgeCostForNodeAtPositionCommandHandler : IRequestHandler<ChangeEdgeCostForNodeAtPositionCommand, bool>
    {
        readonly IMediator _mediator;
        readonly IServerState _serverState;
        public ChangeEdgeCostForNodeAtPositionCommandHandler(
            IMediator mediator,
            IServerState serverState
        )
        {
            _mediator = mediator;
            _serverState = serverState;
        }
        public async Task<bool> Handle(
            ChangeEdgeCostForNodeAtPositionCommand request,
            CancellationToken cancellationToken
        )
        {
            // Lookup node.
            var node = await _mediator.Send(
                new GetMapNodeAtPositionEvent
                {
                    Position = request.Position
                }
            );
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
            var map = (await _serverState.Map());
            foreach (var edge in edges)
            {
                map.RemoveEdge(edge);
            }
            // Save edges back into map
            foreach (var edge in updatedEdges)
            {
                map.AddEdge(edge);
            }

            return true;
        }
    }
}