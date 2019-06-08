using System.Linq;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Math;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Structure;
using EventHorizon.Game.Server.Zone.Model.Map;
using System.Collections.Concurrent;
using System;

namespace EventHorizon.Game.Server.Zone.Map.State
{
    public class MapGraph
    {
        private ConcurrentDictionary<MapNode, MapNode> _nodes;
        private ConcurrentDictionary<MapEdge, MapEdge> _edges;
        private bool _isDirectionGraph;
        private int nextNodeIndex;
        private Octree<MapNode> octree;

        public int NumberOfNodes
        {
            get { return this._nodes.Count; }
        }
        public IList<MapNode> NodeList
        {
            get { return this._nodes.ToList() as IList<MapNode>; }
        }
        public IList<MapEdge> EdgeList
        {
            get { return this._edges.ToList() as IList<MapEdge>; }
        }

        public MapGraph(Vector3 position, Vector3 dimensions, bool isDirectionGraph)
        {
            this._isDirectionGraph = isDirectionGraph;
            this._nodes = new ConcurrentDictionary<MapNode, MapNode>();
            this._edges = new ConcurrentDictionary<MapEdge, MapEdge>();

            this.nextNodeIndex = -1;

            this.octree = new Octree<MapNode>(position, dimensions, 0);
        }

        public IList<MapNode> All()
        {
            return this.octree.All();
        }

        public MapNode GetNode(int index)
        {
            return this._nodes.GetValueOrDefault(new MapNode(index));
        }

        public IList<MapNode> GetClosestNodes(Vector3 position, float radius)
        {
            return this.octree
                .FindNearbyPoints(GetClosestNode(position).Position, radius, null);
        }

        public MapNode GetClosestNode(Vector3 position)
        {
            return this.octree.FindNearestPoint(position, null);
        }

        public MapEdge GetEdge(int from, int to)
        {
            var edge = default(MapEdge);
            this._edges.TryGetValue(
                new MapEdge(from, to),
                out edge
            );
            // foreach (var edge in this._edges)
            // {
            //     if (edge.Value.FromIndex == from
            //         && edge.Value.ToIndex == to)
            //     {
            //         return edge.Value;
            //     }
            // }
            return edge;
        }

        public MapNode AddNode(MapNode node)
        {
            if (this.ContainsNode(node.Index))
            {
                return node;
            }

            this.nextNodeIndex++;
            node.Index = this.nextNodeIndex;
            this._nodes.AddOrUpdate(node, node, (currentKey, key) => node);
            this.octree.Add(node);
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

            // this._edges.Add(edge);

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
                // this._edges.Add(new MapEdge
                // {
                //     ToIndex = edge.FromIndex,
                //     FromIndex = edge.ToIndex
                // });
            }
        }

        public void RemoveEdge(MapEdge edge)
        {
            this._edges.TryRemove(
                edge,
                out _
            );
        }

        public IList<MapEdge> GetEdgesOfNode(int nodeIndex)
        {
            var edgeList = new List<MapEdge>();
            foreach (var edge in this._edges)
            {
                if (edge.Value.FromIndex == nodeIndex)
                {
                    edgeList.Add(edge.Value);
                }
            }
            return edgeList;
        }

        private bool ContainsNode(int index)
        {
            return _nodes.ContainsKey(
                new MapNode(index)
            );
        }
    }
}