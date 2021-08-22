namespace EventHorizon.Zone.System.Player.Events.Connected
{
    using MediatR;

    public struct PlayerDisconnectedEvent : INotification
    {
        public string Id { get; }

        public PlayerDisconnectedEvent(
            string id
        )
        {
            this.Id = id;

        }
    }
}
