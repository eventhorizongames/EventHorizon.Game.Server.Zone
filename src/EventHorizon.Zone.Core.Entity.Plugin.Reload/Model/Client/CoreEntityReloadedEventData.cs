namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.Core.Model.Entity;

    public class CoreEntityReloadedEventData
        : IClientActionData
    {
        public ObjectEntityConfiguration EntityConfiguration { get; }

        public CoreEntityReloadedEventData(
            ObjectEntityConfiguration entityConfiguration
        )
        {
            EntityConfiguration = entityConfiguration;
        }
    }
}
