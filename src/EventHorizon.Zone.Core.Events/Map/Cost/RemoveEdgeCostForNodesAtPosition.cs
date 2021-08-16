namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    using System.Numerics;

    using MediatR;

    public struct RemoveEdgeCostForNodesAtPosition : IRequest<bool>
    {
        public Vector3 Position { get; }
        public Vector3 BoundingBox { get; }
        public int Cost { get; }

        public RemoveEdgeCostForNodesAtPosition(
            Vector3 position,
            Vector3 boundingBox,
            int cost
        )
        {
            Position = position;
            BoundingBox = boundingBox;
            Cost = cost;
        }
    }
}
