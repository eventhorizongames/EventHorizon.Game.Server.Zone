using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Client
{
    public class ClientActionEvent : INotification
    {
        public string PlayerId { get; internal set; }
        public string Action { get; internal set; }
        public object Data { get; internal set; }
    }
}