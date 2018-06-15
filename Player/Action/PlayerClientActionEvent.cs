using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Action
{
    public class PlayerClientActionEvent : INotification
    {
        public string PlayerId { get; internal set; }
        public string Action { get; internal set; }
        public object Data { get; internal set; }
    }
}