namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.Core.Model.Entity;

    public class EntityCoreReloadedEventData
        : IClientActionData
    {
        public ObjectEntityConfiguration EntityConfiguration { get; }

        public EntityCoreReloadedEventData(
            ObjectEntityConfiguration entityConfiguration
        )
        {
            EntityConfiguration = entityConfiguration;
        }
    }
}
