using EventHorizon.Game.Server.Zone.Model.Player;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Update
{
    public class PlayerGlobalUpdateEvent : INotification
    {
        public PlayerEntity Player { get; set; }
    }
}