using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Action
{
    public class PlayerActionEvent : INotification
    {
        public string PlayerId { get; internal set; }
        public string Action { get; internal set; }
        public dynamic Data { get; internal set; }
    }
}