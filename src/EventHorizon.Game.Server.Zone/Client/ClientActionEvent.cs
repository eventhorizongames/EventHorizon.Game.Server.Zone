using MediatR;

namespace EventHorizon.Game.Server.Zone.Client
{
    public struct ClientActionEvent : INotification
    {
        public string Action { get; set; }
        public IClientActionData Data { get; set; }
    }
}