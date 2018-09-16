using System.Linq;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Math;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Map.Model;

namespace EventHorizon.Game.Server.Zone.Map.State
{
    public class MapGraph
    {
        private IList<MapNode> _nodes;
        private IList<MapEdge> _edges;
        private bool _isDirectionGraph;
        private int nextNodeIndex;
        private Octree<MapNode> octree;

        public int NumberOfNodes
        {
            get { return this._nodes.Count; }
        }

        public MapGraph(Vector3 position, Vector3 dimensions, bool isDirectionGraph)
        {
            this._isDirectionGraph = isDirectionGraph;
            this._nodes = new List<MapNode>();
            this._edges = new List<MapEdge>();

            this.nextNodeIndex = -1;

            this.octree = new Octree<MapNode>(position, dimensions, 0);
        }

        public IList<MapNode> All()
        {
            return this.octree.All();
        }

        public MapNode GetNode(int index)
        {
            return this._nodes[index];
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
            foreach (var edge in this._edges)
            {
                if (edge.FromIndex == from
                    && edge.ToIndex == to)
                {
                    return edge;
                }
            }
            return default(MapEdge);
        }

        public MapNode AddNode(MapNode node)
        {
            if (this.ContainsNode(node.Index))
            {
                return node;
            }

            this.nextNodeIndex++;
            node.Index = this.nextNodeIndex;
            this._nodes.Add(node);
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

            this._edges.Add(edge);

            if (!this._isDirectionGraph)
            {
                this._edges.Add(new MapEdge
                {
                    ToIndex = edge.FromIndex,
                    FromIndex = edge.ToIndex
                });
            }
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

        private bool ContainsNode(int index)
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
    }
}