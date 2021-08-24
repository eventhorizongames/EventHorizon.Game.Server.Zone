namespace EventHorizon.Zone.System.Player.Model.Action
{
    using EventHorizon.Zone.Core.Model.Entity;

    public class StandardPlayerAction
        : EntityAction
    {
        public static readonly StandardPlayerAction CONNECTION_ID = new("Player.ConnectionId");
        public static readonly StandardPlayerAction REGISTERED = new("Player.Registered");

        protected StandardPlayerAction(
            string type
        ) : base(type)
        {
        }
    }
}
