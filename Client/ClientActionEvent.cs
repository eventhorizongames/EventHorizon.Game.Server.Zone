using MediatR;

namespace EventHorizon.Game.Server.Zone.Client
{
    public class ClientActionEvent : INotification
    {
        public string Action { get; internal set; }
        public object Data { get; internal set; }
    }
}