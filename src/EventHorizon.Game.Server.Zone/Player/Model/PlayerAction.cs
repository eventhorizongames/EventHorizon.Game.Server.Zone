using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
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