namespace EventHorizon.Game.Server.Zone.Player.Model;

using EventHorizon.Zone.Core.Model.Entity;

public class PlayerAction
    : EntityAction
{
    public static readonly PlayerAction CONNECTION_ID = new("Player.ConnectionId");
    public static readonly PlayerAction REGISTERED = new("Player.Registered");

    protected PlayerAction(
        string type
    ) : base(type)
    {
    }
}
