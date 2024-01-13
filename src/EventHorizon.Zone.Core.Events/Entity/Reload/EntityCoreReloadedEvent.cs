namespace EventHorizon.Zone.Core.Events.Entity.Reload;

using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

public struct EntityCoreReloadedEvent
    : INotification
{
    public ObjectEntityConfiguration EntityConfiguration { get; }

    public EntityCoreReloadedEvent(
        ObjectEntityConfiguration entityConfiguration
    )
    {
        EntityConfiguration = entityConfiguration;
    }
}
