namespace EventHorizon.Zone.Core.Events.Entity.Register;

using EventHorizon.Observer.Model;
using EventHorizon.Zone.Core.Observable;

using MediatR;

/// <summary>
/// UnRegister an entity with the system.
/// </summary>
[ObservableEvent]
public struct EntityUnRegisteredEvent : INotification
{
    public long EntityId { get; set; }

    public EntityUnRegisteredEvent(long entityId)
    {
        EntityId = entityId;
    }
}

public interface EntityUnRegisterEventObserver : ArgumentObserver<EntityUnRegisteredEvent> { }
