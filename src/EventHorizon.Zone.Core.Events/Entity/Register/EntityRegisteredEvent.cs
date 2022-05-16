namespace EventHorizon.Zone.Core.Events.Entity.Register;

using EventHorizon.Observer.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Observable;

using MediatR;

/// <summary>
/// The entity register event.
/// </summary>
[ObservableEvent]
public struct EntityRegisteredEvent : INotification
{
    public IObjectEntity Entity { get; set; }
}

public interface EntityRegisteredEventObserver : ArgumentObserver<EntityRegisteredEvent> { }
