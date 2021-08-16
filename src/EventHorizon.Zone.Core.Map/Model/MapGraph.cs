namespace EventHorizon.Zone.Core.Map.Model
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    using EventHorizon.Zone.Core.Model.Map;
    using EventHorizon.Zone.Core.Model.Math;

    public class MapGraph : IMapGraph
    {
        private readonly IEnumerable<MapEdge> EMPTY_IMMUTABLE_LIST = new List<MapEdge>().AsReadOnly();

        private int _nextNodeIndex;
        private readonly ConcurrentDictionary<MapNode, MapNode> _nodes;
        private readonly ConcurrentDictionary<MapEdge, MapEdge> _edges;
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<MapEdge, MapEdge>> _nodeFromEdges;
        private readonly bool _isDirectionGraph;
        private readonly Octree<MapNode> _octree;

        public int NumberOfNodes
        {
            get { return this._nodes.Count; }
        }

        public IList<MapNode> NodeList
        {
            get { return this._nodes.Values.ToList(); }
        }

        public IList<MapEdge> EdgeList
        {
            get { return this._edges.Values.ToList(); }
        }

        public MapGraph(Vector3 position, Vector3 dimensions, bool isDirectionGraph)
        {
            _isDirectionGraph = isDirectionGraph;
            _nodes = new ConcurrentDictionary<MapNode, MapNode>();
            _edges = new ConcurrentDictionary<MapEdge, MapEdge>();
            _nodeFromEdges = new ConcurrentDictionary<int, ConcurrentDictionary<MapEdge, MapEdge>>();

            _nextNodeIndex = -1;

            _octree = new Octree<MapNode>(position, dimensions, 0);
        }

        public IList<MapNode> All()
        {
            return this._octree.All();
        }

        public MapNode GetNode(int index)
        {
            var mapNode = default(MapNode);
            this._nodes.TryGetValue(
                new MapNode(index),
                out mapNode
            );
            return mapNode;
        }

        public IList<MapNode> GetClosestNodes(Vector3 position, float radius)
        {
            return this._octree
                .FindNearbyPoints(GetClosestNode(position).Position, radius, null);
        }

        public IList<MapNode> GetClosestNodesInDimension(
            Vector3 position,
            Vector3 dimensions
        )
        {
            return this._octree.FindNearbyPoints(
                position,
                dimensions,
                null
            );
        }

        public MapNode GetClosestNode(Vector3 position)
        {
            return this._octree.FindNearestPoint(position, null);
        }

        public MapEdge GetEdge(int from, int to)
        {
            var edge = default(MapEdge);
            this._edges.TryGetValue(
                new MapEdge(from, to),
                out edge
            );
            return edge;
        }

        public MapNode AddNode(MapNode node)
        {
            if (this.ContainsNode(node.Index))
            {
                return node;
            }

            this._nextNodeIndex++;
            node.Index = this._nextNodeIndex;
            this._nodes.AddOrUpdate(
                node,
                node,
                (_, __) => node
            );
            this._octree.Add(node);

            return node;
        }

        public void AddEdge(MapEdge edge)
        {
            // Validate to and from
            if (!this.ContainsNode(edge.FromIndex)
                || !this.ContainsNode(edge.ToIndex))
            {
                return;
            }
            this._edges.AddOrUpdate(
                edge,
                edge,
                (key, current) => edge
            );
            AddFromNodeToEdge(
                edge
            );

            if (!this._isDirectionGraph)
            {
                var reversedEdge = new MapEdge
                {
                    ToIndex = edge.FromIndex,
                    FromIndex = edge.ToIndex
                };
                this._edges.AddOrUpdate(
                    reversedEdge,
                    reversedEdge,
                    (key, current) => reversedEdge
                );
                AddFromNodeToEdge(
                    reversedEdge
                );
            }
        }

        private void AddFromNodeToEdge(
            MapEdge edge
        )
        {
            this._nodeFromEdges
                .GetOrAdd(
                    edge.FromIndex,
                    new ConcurrentDictionary<MapEdge, MapEdge>()
                ).TryAdd(
                    edge,
                    edge
                );
        }

        public void RemoveEdge(MapEdge edge)
        {
            this._edges.TryRemove(
                edge,
                out _
            );
            this._nodeFromEdges
                .GetOrAdd(
                    edge.FromIndex,
                    new ConcurrentDictionary<MapEdge, MapEdge>()
                ).TryRemove(
                    edge,
                    out _
                );
        }

        public IEnumerable<MapEdge> GetEdgesOfNode(int nodeIndex)
        {
            ConcurrentDictionary<MapEdge, MapEdge> edgeList;
            if (this._nodeFromEdges
                .TryGetValue(
                    nodeIndex,
                    out edgeList
                )
            )
            {
                return edgeList.Values;
            }

            return EMPTY_IMMUTABLE_LIST;
        }

        private bool ContainsNode(int index)
        {
            return _nodes.ContainsKey(
                new MapNode(index)
            );
        }
    }
}
