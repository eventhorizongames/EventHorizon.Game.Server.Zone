namespace EventHorizon.Zone.System.Agent.Save.Mapper
{
    using EventHorizon.Zone.System.ClientEntities.Model;

    public static class ClientEntityFromDetailsToEntity
    {
        public static ClientEntity Map(
            ClientEntityDetails details
        )
        {
            return new ClientEntity(
                details.Id,
                details.Data
            )
            {
                Id = -1,
                Name = details.Name,
                Transform = details.Transform,
                TagList = details.TagList,
            };
        }
    }
}
