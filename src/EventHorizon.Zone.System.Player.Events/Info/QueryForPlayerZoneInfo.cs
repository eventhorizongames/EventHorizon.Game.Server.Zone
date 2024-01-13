namespace EventHorizon.Zone.System.Player.Events.Info;

using global::System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Player;

using MediatR;

public struct QueryForPlayerZoneInfo : IRequest<IDictionary<string, object>>
{
    public PlayerEntity Player { get; }
    public QueryForPlayerZoneInfo(
        PlayerEntity player
    )
    {
        Player = player;
    }
}
