namespace EventHorizon.Zone.System.Player.Plugin.Action.Events;

using global::System.Collections.Generic;

using MediatR;

public struct RunPlayerServerAction
    : INotification
{
    public string PlayerId { get; }
    public string Action { get; }
    public IDictionary<string, object> Data { get; }

    public RunPlayerServerAction(
        string playerId,
        string action,
        IDictionary<string, object> data
    )
    {
        PlayerId = playerId;
        Action = action;
        Data = data;
    }
}
