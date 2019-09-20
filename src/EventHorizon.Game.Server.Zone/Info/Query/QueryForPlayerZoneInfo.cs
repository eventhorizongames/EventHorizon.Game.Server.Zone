using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Info.Api;
using EventHorizon.Zone.Core.Model.Player;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForPlayerZoneInfo : IRequest<IDictionary<string, object>>
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