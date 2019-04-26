using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Map.Cost
{
    public struct ChangeEdgeCostForNodeAtPositionCommand : IRequest<bool>
    {
        public Vector3 Position { get; }
        public int Cost { get; }
        public ChangeEdgeCostForNodeAtPositionCommand(
            Vector3 position,
            int cost
        )
        {
            this.Position = position;
            this.Cost = cost;
        }
    }
}