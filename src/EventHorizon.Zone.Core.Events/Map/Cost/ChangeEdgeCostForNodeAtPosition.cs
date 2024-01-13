namespace EventHorizon.Zone.Core.Events.Map.Cost;

using System.Numerics;

using MediatR;

public struct ChangeEdgeCostForNodeAtPosition : IRequest<bool>
{
    public Vector3 Position { get; }
    public int Cost { get; }

    public ChangeEdgeCostForNodeAtPosition(
        Vector3 position,
        int cost
    )
    {
        Position = position;
        Cost = cost;
    }
}
