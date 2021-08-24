namespace EventHorizon.Zone.Core.Model.Map
{
    using System.Collections.Generic;
    using System.Numerics;

    public interface IMapGraph
    {
        int NumberOfNodes { get; }
        IList<MapNode> NodeList { get; }
        IList<MapEdge> EdgeList { get; }

        IList<MapNode> All();
        // TODO: Make Nullable
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
        // TODO: Make Nullable
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
