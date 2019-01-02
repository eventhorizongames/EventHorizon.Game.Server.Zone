
using System;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;

namespace EventHorizon.Game.Server.Zone.Agent.Mapper
{
    public class AgentFromDetailsToEntity
    {
        public static AgentEntity MapToNew(AgentDetails details)
        {
            // TODO: Add validation to details.
            return new AgentEntity
            {
                Id = -1,
                AgentId = details.Id,
                Type = EntityType.AGENT,
                Name = details.Name,
                Position = new PositionState
                {
                    CanMove = true,
                    CurrentPosition = details.Position.CurrentPosition,
                    CurrentZone = details.Position.CurrentZone,
                    ZoneTag = details.Position.ZoneTag,
                    
                    // NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
                    MoveToPosition = details.Position.CurrentPosition,
                },
                TagList = details.TagList,
                RawData = details.Data,
            };
        }
    }
}