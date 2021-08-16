namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    using System.Numerics;

    using MediatR;

    public struct RemoveEdgeCostForNodeAtPosition : IRequest<bool>
    {
        public Vector3 Position { get; }
        public int Cost { get; }

        public RemoveEdgeCostForNodeAtPosition(
            Vector3 position,
            int cost
        )
        {
            Position = position;
            Cost = cost;
        }
    }
}
