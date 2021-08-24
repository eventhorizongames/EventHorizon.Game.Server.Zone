namespace EventHorizon.Zone.System.Player.Events.Connected
{
    using MediatR;

    public struct PlayerConnectedEvent : INotification
    {
        public string Id { get; }
        public string ConnectionId { get; }

        public PlayerConnectedEvent(
            string id,
            string connectionId
        )
        {
            Id = id;
            ConnectionId = connectionId;
        }
    }
}
