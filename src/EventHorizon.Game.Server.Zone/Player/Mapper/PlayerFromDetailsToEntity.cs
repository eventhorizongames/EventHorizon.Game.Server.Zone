
using System.Collections.Generic;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;

namespace EventHorizon.Game.Server.Zone.Player.Mapper
{
    public class PlayerFromDetailsToEntity
    {
        public static PlayerEntity MapToNew(PlayerDetails details)
        {
            return new PlayerEntity
            {
                Id = -1,
                PlayerId = details.Id,
                Name = details.Name,
                Locale = details.Locale,
                ConnectionId = GetConnectionId(details),
                Type = EntityType.PLAYER,
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = details.Position.Position,
                    // NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
                    MoveToPosition = details.Position.Position,
                    CurrentZone = details.Position.CurrentZone,
                    ZoneTag = details.Position.ZoneTag,
                },
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