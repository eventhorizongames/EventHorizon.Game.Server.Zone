using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    public struct ChangeEdgeCostForNodesAtPositionCommand : IRequest<bool>
    {
        public Vector3 Position { get; }
        public Vector3 BoundingBox { get; }
        public int Cost { get; }
        public ChangeEdgeCostForNodesAtPositionCommand(
            Vector3 position,
            Vector3 boundingBox,
            int cost
        )
        {
            this.Position = position;
            BoundingBox = boundingBox;
            this.Cost = cost;
        }
    }
}