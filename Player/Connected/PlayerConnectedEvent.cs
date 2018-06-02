using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Connected
{
    public class PlayerConnectedEvent : INotification
    {
        public string Id { get; internal set; }
        public string ConnectionId { get; internal set; }
    }
}