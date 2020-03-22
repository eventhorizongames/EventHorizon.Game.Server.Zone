
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Model.Details;

namespace EventHorizon.Zone.System.Player.Mapper
{
    public class PlayerFromDetailsToEntity
    {
        public static PlayerEntity MapToNew(
            PlayerDetails details
        )
        {
            return new PlayerEntity
            {
                Id = -1,
                PlayerId = details.Id,
                Name = details.Name,
                Locale = details.Locale,
                ConnectionId = GetConnectionId(details),
                Type = EntityType.PLAYER,
                Transform = details.Transform,
                RawData = details.Data,
                TagList = new List<string> { "player" }
            };
        }

        private static string GetConnectionId(PlayerDetails details)
        {
            if (details.Data.ContainsKey("ConnectionId"))
            {
                return (string)details.Data["ConnectionId"];
            }
            return "";
        }
    }
}