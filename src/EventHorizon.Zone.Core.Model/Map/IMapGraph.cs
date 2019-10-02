using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Zone.Core.Model.Map
{
    public interface IMapGraph
    {
        int NumberOfNodes { get; }
        IList<MapNode> NodeList { get; }
        IList<MapEdge> EdgeList { get; }

        IList<MapNode> All();
        MapNode GetNode(
            int index
        );
        IList<MapNode> GetClosestNodes(
            Vector3 position,
            float radius
        );
        IList<MapNode> GetClosestNodesInDimension(
            Vector3 position,
            Vector3 dimensions
        );
        MapNode GetClosestNode(
            Vector3 position
        );
        MapEdge GetEdge(
            int from,
            int to
        );
        MapNode AddNode(
            MapNode node
        );
        void AddEdge(
            MapEdge edge
        );
        void RemoveEdge(
            MapEdge edge
        );
        IEnumerable<MapEdge> GetEdgesOfNode(
            int nodeIndex
        );
    }
}