
using System;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;

namespace EventHorizon.Zone.System.Agent.Mapper
{
    public class AgentFromDetailsToEntity
    {
        public static AgentEntity MapToNewGlobal(AgentDetails details)
        {
            return MapToNew(details, details.Id, true);
        }

        public static AgentEntity MapToNew(AgentDetails details, string agentId, bool isGlobal = false)
        {
            // TODO: Add validation to details.
            return new AgentEntity
            {
                Id = -1,
                IsGlobal = isGlobal,
                AgentId = agentId,
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