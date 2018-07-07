using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Update
{
    public class PlayerGlobalUpdateEvent : INotification
    {
        public PlayerEntity Player { get; internal set; }
    }
}