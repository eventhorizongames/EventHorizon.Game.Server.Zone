
using System;
using System.Collections.Generic;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.Model;

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
                ConnectionId = details.Data.ConnectionId,
                Type = EntityType.PLAYER,
                Position = new PositionState
                {
                    CurrentPosition = details.Position.Position,
                    NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
                    MoveToPosition = details.Position.Position,
                    CurrentZone = details.Position.CurrentZone,
                    ZoneTag = details.Position.ZoneTag,
                },
                Data = details.Data,
                TagList = new List<string> { "player" }
            };
        }
    }
}