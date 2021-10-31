namespace EventHorizon.Zone.Core.Events.Entity.Reload
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct CoreEntityReloadedEvent
        : INotification
    {
        public ObjectEntityConfiguration EntityConfiguration { get; }

        public CoreEntityReloadedEvent(
            ObjectEntityConfiguration entityConfiguration
        )
        {
            EntityConfiguration = entityConfiguration;
        }
    }
}
