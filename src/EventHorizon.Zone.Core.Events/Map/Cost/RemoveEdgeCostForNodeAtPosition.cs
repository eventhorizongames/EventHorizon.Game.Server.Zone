namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    using System.Numerics;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public struct RemoveEdgeCostForNodeAtPosition : IRequest<bool>
    {
        public bool IsNode { get; }
        public MapNode Node { get; }
        public Vector3 Position { get; }
        public int Cost { get; }

        public RemoveEdgeCostForNodeAtPosition(
            MapNode node,
            int cost
        )
        {
            IsNode = true;
            Node = node;
            Position = default(Vector3);
            Cost = cost;
        }

        public RemoveEdgeCostForNodeAtPosition(
            Vector3 position,
            int cost
        )
        {
            IsNode = false;
            Node = default(MapNode);
            Position = position;
            Cost = cost;
        }
    }
}