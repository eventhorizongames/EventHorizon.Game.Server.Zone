using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Connected
{
    public class PlayerDisconnectedEvent : INotification
    {
        public string Id { get; internal set; }
    }
}