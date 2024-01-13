namespace EventHorizon.Zone.System.Agent.Save.Mapper;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;

public static class ClientEntityFromEntityToDetails
{
    public static ClientEntityDetails Map(
        ClientEntity entity
    )
    {
        return new ClientEntityDetails
        {
            Id = entity.ClientEntityId,
            Name = entity.Name,
            Transform = entity.Transform,
            TagList = entity.TagList,
            Data = entity.AllData(),
        };
    }
}
