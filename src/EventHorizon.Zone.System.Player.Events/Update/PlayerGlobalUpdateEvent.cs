namespace EventHorizon.Zone.System.Player.Events.Update
{
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    public struct PlayerGlobalUpdateEvent
        : INotification
    {
        public PlayerEntity Player { get; }

        public PlayerGlobalUpdateEvent(
            PlayerEntity player
        )
        {
            Player = player;
        }
    }
}
