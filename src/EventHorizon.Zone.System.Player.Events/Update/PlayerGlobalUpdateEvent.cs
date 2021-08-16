using EventHorizon.Zone.Core.Model.Player;

using MediatR;

namespace EventHorizon.Zone.System.Player.Events.Update
{
    public struct PlayerGlobalUpdateEvent : INotification
    {
        public PlayerEntity Player { get; }

        public PlayerGlobalUpdateEvent(
            PlayerEntity player
        )
        {
            this.Player = player;
        }
    }
}
