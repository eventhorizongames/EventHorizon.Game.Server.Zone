namespace EventHorizon.Zone.System.Agent.Save.Mapper;

using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Model;

public class AgentFromDetailsToEntity
{
    public static AgentEntity MapToNewGlobal(
        AgentDetails details
    )
    {
        return MapToNew(
            details,
            details.Id,
            true
        );
    }

    public static AgentEntity MapToNew(
        AgentDetails details,
        string agentId,
        bool isGlobal = false
    )
    {
        var entity = new AgentEntity(
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
        entity.SetProperty(
            LocationState.PROPERTY_NAME,
            LocationState.New(
                details.Location.CurrentZone,
                details.Location.ZoneTag
            )
        );
        return entity;
    }
}
