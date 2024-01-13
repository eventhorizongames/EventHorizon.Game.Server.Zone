namespace EventHorizon.Zone.Core.Map.Model;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.Core.Model.Math;

public class MapGraph : IMapGraph
{
    private readonly IEnumerable<MapEdge> _emptyImmutableList =
        new List<MapEdge>().AsReadOnly();

    private int _nextNodeIndex;
    private readonly ConcurrentDictionary<MapNode, MapNode> _nodes;
    private readonly ConcurrentDictionary<MapEdge, MapEdge> _edges;
    private readonly ConcurrentDictionary<
        int,
        ConcurrentDictionary<MapEdge, MapEdge>
    > _nodeFromEdges;
    private readonly bool _isDirectionGraph;
    private readonly Octree<MapNode> _octree;

    public int NumberOfNodes
    {
        get { return _nodes.Count; }
    }

    public IList<MapNode> NodeList
    {
        get { return _nodes.Values.ToList(); }
    }

    public IList<MapEdge> EdgeList
    {
        get { return _edges.Values.ToList(); }
    }

    public MapGraph(Vector3 position, Vector3 dimensions, bool isDirectionGraph)
    {
        _isDirectionGraph = isDirectionGraph;
        _nodes = new ConcurrentDictionary<MapNode, MapNode>();
        _edges = new ConcurrentDictionary<MapEdge, MapEdge>();
        _nodeFromEdges =
            new ConcurrentDictionary<int, ConcurrentDictionary<MapEdge, MapEdge>>();

        _nextNodeIndex = -1;

        _octree = new Octree<MapNode>(position, dimensions, 0);
    }

    public IList<MapNode> All()
    {
        return _octree.All();
    }

    public MapNode GetNode(int index)
    {
        _nodes.TryGetValue(new MapNode(index), out var mapNode);
        return mapNode;
    }

    public IList<MapNode> GetClosestNodes(Vector3 position, float radius)
    {
        return _octree.FindNearbyPoints(GetClosestNode(position).Position, radius, null);
    }

    public IList<MapNode> GetClosestNodesInDimension(Vector3 position, Vector3 dimensions)
    {
        return _octree.FindNearbyPoints(position, dimensions, null);
    }

    public MapNode GetClosestNode(Vector3 position)
    {
        return _octree.FindNearestPoint(position, null);
    }

    public MapEdge GetEdge(int from, int to)
    {
        _edges.TryGetValue(new MapEdge(from, to), out var edge);
        return edge;
    }

    public MapNode AddNode(MapNode node)
    {
        if (ContainsNode(node.Index))
        {
            return node;
        }

        _nextNodeIndex++;
        node.Index = _nextNodeIndex;
        _nodes.AddOrUpdate(node, node, (_, _) => node);
        _octree.Add(node);

        return node;
    }

    public void AddEdge(MapEdge edge)
    {
        // Validate to and from
        if (!ContainsNode(edge.FromIndex) || !ContainsNode(edge.ToIndex))
        {
            return;
        }
        _edges.AddOrUpdate(edge, edge, (key, current) => edge);
        AddFromNodeToEdge(edge);

        if (!_isDirectionGraph)
        {
            var reversedEdge = new MapEdge
            {
                ToIndex = edge.FromIndex,
                FromIndex = edge.ToIndex
            };
            _edges.AddOrUpdate(reversedEdge, reversedEdge, (key, current) => reversedEdge);
            AddFromNodeToEdge(reversedEdge);
        }
    }

    private void AddFromNodeToEdge(MapEdge edge)
    {
        _nodeFromEdges
            .GetOrAdd(edge.FromIndex, new ConcurrentDictionary<MapEdge, MapEdge>())
            .TryAdd(edge, edge);
    }

    public void RemoveEdge(MapEdge edge)
    {
        _edges.TryRemove(edge, out _);
        _nodeFromEdges
            .GetOrAdd(edge.FromIndex, new ConcurrentDictionary<MapEdge, MapEdge>())
            .TryRemove(edge, out _);
    }

    public IEnumerable<MapEdge> GetEdgesOfNode(int nodeIndex)
    {
        if (_nodeFromEdges.TryGetValue(nodeIndex, out var edgeList))
        {
            return edgeList.Values;
        }

        return _emptyImmutableList;
    }

    private bool ContainsNode(int index)
    {
        return _nodes.ContainsKey(new MapNode(index));
    }
}
