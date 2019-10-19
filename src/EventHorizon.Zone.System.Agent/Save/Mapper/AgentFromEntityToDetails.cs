using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Connection.Model;

namespace EventHorizon.Zone.System.Agent.Save.Mapper
{
    public class AgentFromEntityToDetails
    {
        public static AgentDetails Map(AgentEntity entity)
        {
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