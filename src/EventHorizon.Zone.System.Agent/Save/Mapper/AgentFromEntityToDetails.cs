namespace EventHorizon.Zone.System.Agent.Save.Mapper;

using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Model;

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
            Location = entity.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            ),
            TagList = entity.TagList,
            Data = entity.AllData(),
        };
    }
}
