using MediatR;

namespace EventHorizon.Zone.System.Player.Plugin.Action
{
    public class PlayerActionEvent : INotification
    {
        public string PlayerId { get; }
        public string Action { get; }
        public dynamic Data { get; }

        public PlayerActionEvent(
            string playerId,
            string action,
            dynamic data
        )
        {
            this.PlayerId = playerId;
            this.Action = action;
            this.Data = data;

        }
    }
}