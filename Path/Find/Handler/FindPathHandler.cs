using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Loop.Map;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Path.Find.Handler
{
    public class FindPathHandler : IRequestHandler<FindPathEvent, Queue<Vector3>>
    {
        readonly IMediator _mediator;
        public FindPathHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Queue<Vector3>> Handle(FindPathEvent request, CancellationToken cancellationToken)
        {

            var fromMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = request.From,
            });
            var toMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = request.To,
            });

            return await this.CreatePath(fromMapNode, toMapNode);
        }

        private async Task<Queue<Vector3>> CreatePath(MapNode fromMapNode, MapNode toMapNode)
        {
            var nodeLength = await _mediator.Send(new GetMapNodeCountEvent());
            var _gCosts = new float[nodeLength];
            var _fCosts = new float[nodeLength];

            var _shortestPathTree = new MapEdge[nodeLength];
            var _searchFrontier = new MapEdge[nodeLength];

            var _source = fromMapNode.Index;
            var _target = toMapNode.Index;
            for (int i = 0; i < nodeLength; i++)
            {
                _gCosts[i] = 0.0f;
                _fCosts[i] = 0.0f;
                _shortestPathTree[i] = MapEdge.NULL;
                _searchFrontier[i] = MapEdge.NULL;
            }

            // AStar Search
            var pq = new BinaryHeap<int>(new MyComparer(_fCosts));

            pq.Enqueue(_source);

            while (pq.Count > 0)
            {
                var nextClosestNode = pq.Dequeue();

                _shortestPathTree[nextClosestNode] = _searchFrontier[nextClosestNode];

                if (nextClosestNode == _target)
                {
                    break;
                }

                // Get/Check all edges attached to the node
                var edgeList = await _mediator.Send(new GetMapEdgesOfNodeEvent
                {
                    NodeIndex = nextClosestNode,
                });

                foreach (var edge in edgeList)
                {
                    // Calculate the heuristic cost from this node to the target (H)
                    var hCost = await HeuristicEuclidCalculate(_target, edge.ToIndex);
                    // Calculate the 'real' cost to this node from the source
                    var gCost = _gCosts[nextClosestNode] + edge.Cost;

                    if (_searchFrontier[edge.ToIndex].Equals(MapEdge.NULL))
                    {
                        _fCosts[edge.ToIndex] = gCost + hCost;
                        _gCosts[edge.ToIndex] = gCost;

                        pq.Add(edge.ToIndex);

                        _searchFrontier[edge.ToIndex] = edge;
                    }
                    else if ((gCost < _gCosts[edge.ToIndex]) && (_shortestPathTree[edge.ToIndex].Equals(MapEdge.NULL)))
                    {
                        _fCosts[edge.ToIndex] = gCost + hCost;
                        _gCosts[edge.ToIndex] = gCost;

                        pq.ReScoreElement(edge.ToIndex);

                        _searchFrontier[edge.ToIndex] = edge;
                    }
                }
            }


            if (_target < 0)
            {
                return new Queue<Vector3>();
            }

            var path = new List<Vector3>();
            var nodeIndex = _target;

            path.Add((await _mediator.Send(new GetMapNodeAtIndexEvent
            {
                NodeIndex = nodeIndex
            })).Position);

            while ((nodeIndex != _source) && (!_shortestPathTree[nodeIndex].Equals(MapEdge.NULL)))
            {
                nodeIndex = _shortestPathTree[nodeIndex].FromIndex;
                path.Add((await _mediator.Send(new GetMapNodeAtIndexEvent
                {
                    NodeIndex = nodeIndex
                })).Position);
            }
            path.Reverse();

            return new Queue<Vector3>(path);
        }

        // HeuristicEuclid
        private async Task<float> HeuristicEuclidCalculate(int node1Index, int node2Index)
        {
            var node1 = await _mediator.Send(new GetMapNodeAtIndexEvent
            {
                NodeIndex = node1Index
            });
            var node2 = await _mediator.Send(new GetMapNodeAtIndexEvent
            {
                NodeIndex = node2Index
            });
            return Vector3.Distance(node1.Position, node2.Position);
        }
    }
    class MyComparer : IComparer<int>
    {
        private float[] _costs;
        public MyComparer(float[] costs)
        {
            _costs = costs;
        }
        public int Compare(int x, int y)
        {
            return (int)(_costs[x] - _costs[y]);
        }
    }
}