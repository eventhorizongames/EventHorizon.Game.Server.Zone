using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Map.Find.Search.Collections;
using EventHorizon.Zone.Core.Model.Map;

namespace EventHorizon.Zone.Core.Map.Search
{
    public static class AStarSearch
    {
        public static Queue<Vector3> CreatePath(
            IMapGraph mapGraph,
            MapNode fromMapNode,
            MapNode toMapNode
        )
        {
            var nodeLength = mapGraph.All().Count;
            var g_score = new float[nodeLength];
            var f_score = new float[nodeLength];

            var path = new Dictionary<int, int>();
            var closedSet = new List<MapNode>();

            var _source = fromMapNode.Index;
            var _target = toMapNode.Index;
            for (int i = 0; i < nodeLength; i++)
            {
                g_score[i] = float.PositiveInfinity;
                f_score[i] = float.PositiveInfinity;
            }

            // AStar Search
            var openSet = new BinaryHeap<int>(new MyComparer(f_score));

            openSet.Enqueue(_source);
            g_score[_source] = 0;
            f_score[_source] = HeuristicEuclidCalculate(
                mapGraph,
                _source,
                _target
            );


            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == _target)
                {
                    break;
                }

                closedSet.Add(mapGraph.GetNode(current));

                // Get/Check all edges attached to the node
                var edgeList = mapGraph.GetEdgesOfNode(
                    current
                );

                foreach (var neighborEdge in edgeList)
                {
                    var neighbor = mapGraph.GetNode(neighborEdge.ToIndex);
                    if (float.IsInfinity(neighborEdge.Cost))
                    {
                        continue;
                    }
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // Calculate the 'real' cost to this node from the source
                    var tentative_g_score = g_score[current] + neighborEdge.Cost;
                    if (openSet.Contains(neighbor.Index) && tentative_g_score >= g_score[neighbor.Index])
                    {
                        continue;
                    }
                    if (!path.ContainsKey(neighbor.Index))
                    {
                        path.Add(neighbor.Index, current);
                    }
                    // Calculate the heuristic cost from this node to the target (H)
                    g_score[neighbor.Index] = tentative_g_score;
                    f_score[neighbor.Index] = g_score[neighbor.Index] + HeuristicEuclidCalculate(mapGraph, neighbor.Index, _target);

                    if (!openSet.Contains(neighbor.Index))
                    {
                        openSet.Enqueue(neighbor.Index);
                    }
                }
            }


            if (_target < 0)
            {
                return new Queue<Vector3>();
            }

            var reversedPath = new List<Vector3>();
            var nodeIndex = _target;
            reversedPath.Add(mapGraph.GetNode(_target).Position);
            while (path.ContainsKey(nodeIndex))
            {
                nodeIndex = path[nodeIndex];
                reversedPath.Add(mapGraph.GetNode(nodeIndex).Position);
            }
            reversedPath.Reverse();
            return new Queue<Vector3>(reversedPath);
        }

        // HeuristicEuclid
        private static float HeuristicEuclidCalculate(
            IMapGraph mapGraph,
            int node1Index,
            int node2Index
        )
        {
            var node1 = mapGraph.GetNode(node1Index);
            var node2 = mapGraph.GetNode(node2Index);
            return Vector3.Distance(node1.Position, node2.Position);
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
}