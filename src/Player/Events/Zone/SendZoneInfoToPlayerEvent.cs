using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Zone
{
    public class SendZoneInfoToPlayerEvent : IRequest<PlayerEntity>
    {
        public PlayerEntity Player { get; internal set; }
    }
}