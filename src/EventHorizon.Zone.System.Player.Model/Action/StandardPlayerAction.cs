using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Player.Model.Action
{
    public class StandardPlayerAction : EntityAction
    {
        public static readonly StandardPlayerAction CONNECTION_ID = new StandardPlayerAction("Player.ConnectionId");
        public static readonly StandardPlayerAction REGISTERED = new StandardPlayerAction("Player.Registered");

        protected StandardPlayerAction(string type)
            : base(type)
        {
        }
    }
}