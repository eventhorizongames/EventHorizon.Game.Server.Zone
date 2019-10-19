
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Connection.Model;

namespace EventHorizon.Zone.System.Agent.Save.Mapper
{
    public class AgentFromDetailsToEntity
    {
        public static AgentEntity MapToNewGlobal(
            AgentDetails details
        ) => MapToNew(
                details,
                details.Id,
                true
            );

        public static AgentEntity MapToNew(
            AgentDetails details, 
            string agentId, 
            bool isGlobal = false
        )
        {
            return new AgentEntity(
                details.Data
            )
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

                    MoveToPosition = details.Position.CurrentPosition,
                },
                TagList = details.TagList
            };
        }
    }
}