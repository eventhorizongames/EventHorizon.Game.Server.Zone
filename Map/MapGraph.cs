using System.Linq;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Math;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Map
{
    public class MapGraph
    {
        private IList<MapNode> _nodes;
        private IList<MapEdge> _edges;
        private bool _isDirectionGraph;
        private int nextNodeIndex;
        private Octree<MapNode> octree;

        public IList<MapNode> Nodes
        {
            get { return _nodes; }
        }
        public IList<MapEdge> Edges
        {
            get { return _edges; }
        }
        public int NumberOfNodes
        {
            get { return this._nodes.Count; }
        }
        public int NumberOfEdges
        {
            get { return this._edges.Count; }
        }
        public bool IsEmpty
        {
            get { return this._nodes.Count == 0; }
        }

        public MapGraph(Vector3 position, Vector3 dimensions, bool isDirectionGraph)
        {
            this._isDirectionGraph = isDirectionGraph;
            this._nodes = new List<MapNode>();
            this._edges = new List<MapEdge>();

            this.nextNodeIndex = -1;

            this.octree = new Octree<MapNode>(position, dimensions, 0);
        }

        public MapNode GetNode(int index)
        {
            return this._nodes[index];
        }

        public IList<MapNode> GetClosestNodes(Vector3 position, float distance)
        {
            return this.octree
                .FindNearbyPoints(this.octree.FindNearestPoint(position, null).Position, distance, null);
        }

        public MapEdge GetEdge(int from, int to)
        {
            foreach (var edge in this._edges)
            {
                if (edge.FromIndex == from
                    && edge.ToIndex == to)
                {
                    return edge;
                }
            }
            return null;
        }

        public int AddNode(MapNode node)
        {
            if (this.ContainsNode(node.Index))
            {
                return node.Index;
            }

            this.nextNodeIndex++;
            node.Index = this.nextNodeIndex;
            this._nodes.Add(node);
            this.octree.Add(node);
            return this.nextNodeIndex;
        }
        public void AddEdge(MapEdge edge)
        {
            // Validate to and from
            if (!this.ContainsNode(edge.FromIndex)
                || !this.ContainsNode(edge.ToIndex))
            {
                return;
            }

            this._edges.Add(edge);

            if (!this._isDirectionGraph)
            {
                this._edges.Add(new MapEdge
                {
                    ToIndex = edge.ToIndex,
                    FromIndex = edge.FromIndex
                });
            }
        }

        public bool ContainsNode(int index)
        {
            foreach (var node in this._nodes)
            {
                if (node.Index == index)
                {
                    return true;
                }
            }
            return false;
        }

        public IList<MapEdge> GetEdgesOfNode(int nodeIndex)
        {
            var edgeList = new List<MapEdge>();
            foreach (var edge in this._edges)
            {
                if (edge.FromIndex == nodeIndex)
                {
                    edgeList.Add(edge);
                }
            }
            return edgeList;
        }
    }
}