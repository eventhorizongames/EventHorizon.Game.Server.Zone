using EventHorizon.Game.Server.Zone.Info.Api;
using EventHorizon.Game.Server.Zone.Model.Player;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForPlayerZoneInfo : IRequest<IZoneInfo>
    {
        public PlayerEntity Player { get; }
        public QueryForPlayerZoneInfo(
            PlayerEntity player
        )
        {
            Player = player;
        }
    }
}