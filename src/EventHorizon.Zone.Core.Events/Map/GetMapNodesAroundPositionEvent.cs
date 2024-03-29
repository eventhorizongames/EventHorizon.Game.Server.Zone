namespace EventHorizon.Zone.Core.Events.Map;

using System.Collections.Generic;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Map;

using MediatR;

public struct GetMapNodesAroundPositionEvent : IRequest<IList<MapNode>>
{
    public Vector3 Position { get; }
    public int Distance { get; }

    public GetMapNodesAroundPositionEvent(
        Vector3 position,
        int distance
    )
    {
        Position = position;
        Distance = distance;
    }
}
