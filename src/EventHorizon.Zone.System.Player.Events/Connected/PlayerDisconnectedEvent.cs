using MediatR;

namespace EventHorizon.Zone.System.Player.Events.Connected
{
    public class PlayerDisconnectedEvent : INotification
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