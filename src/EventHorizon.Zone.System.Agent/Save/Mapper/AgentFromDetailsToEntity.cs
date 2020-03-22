namespace EventHorizon.Zone.System.Agent.Save.Mapper
{
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.Core.Model.Core;

    public class AgentFromDetailsToEntity
    {
        public static AgentEntity MapToNewGlobal(
            AgentDetails details
        )
        {
            var entity = MapToNew(
                details,
                details.Id,
                true
            );
            entity.SetProperty(
                LocationState.PROPERTY_NAME,
                details.Location
            );
            return entity;
        }

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
                Transform = details.Transform,
                TagList = details.TagList
            };
        }
    }
}