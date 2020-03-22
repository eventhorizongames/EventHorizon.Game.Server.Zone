namespace EventHorizon.Zone.System.Agent.Save.Mapper
{
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.Core.Model.Core;

    public class AgentFromEntityToDetails
    {
        public static AgentDetails Map(
            AgentEntity entity
        )
        {
            return new AgentDetails
            {
                Id = entity.AgentId,
                Name = entity.Name,
                Transform = entity.Transform,
                Location = entity.GetProperty<LocationState>(
                    LocationState.PROPERTY_NAME
                ),
                TagList = entity.TagList,
                Data = entity.AllData(),
            };
        }
    }
}