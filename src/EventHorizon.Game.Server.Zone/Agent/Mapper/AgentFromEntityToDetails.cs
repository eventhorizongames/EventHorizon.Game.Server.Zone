using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Zone.System.Agent.Mapper
{
    public class AgentFromEntityToDetails
    {
        public static AgentDetails Map(AgentEntity entity)
        {
            // TODO: Add validation to Entity.
            return new AgentDetails
            {
                Id = entity.AgentId,
                Name = entity.Name,
                Position = entity.Position,
                TagList = entity.TagList,
                Data = entity.AllData(),
            };
        }
    }
}