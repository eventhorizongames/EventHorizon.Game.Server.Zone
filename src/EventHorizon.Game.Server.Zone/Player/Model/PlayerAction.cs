namespace EventHorizon.Game.Server.Zone.Player.Model
{
    using EventHorizon.Game.Server.Zone.Entity.Model;
    using EventHorizon.Zone.Core.Model.Entity;

    public class PlayerAction : EntityAction
    {
        public static readonly PlayerAction CONNECTION_ID = new PlayerAction("Player.ConnectionId");
        public static readonly PlayerAction REGISTERED = new PlayerAction("Player.Registered");

        protected PlayerAction(string type)
            : base(type)
        {
        }
    }
}
