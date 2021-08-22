namespace EventHorizon.Zone.System.Player.Events.Zone
{
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    public class SendZoneInfoToPlayerEvent
        : IRequest<PlayerEntity>
    {
        public PlayerEntity Player { get; }

        public SendZoneInfoToPlayerEvent(
            PlayerEntity player
        )
        {
            Player = player;
        }
    }
}
