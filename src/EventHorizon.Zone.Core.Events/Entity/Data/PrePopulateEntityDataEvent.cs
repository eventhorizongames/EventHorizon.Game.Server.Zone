namespace EventHorizon.Zone.Core.Events.Entity.Data;

using EventHorizon.Observer.Model;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Observable;

using MediatR;

/// <summary>
/// The event sent before an Entity is created to fill Entity data.
/// </summary>
[ObservableEvent]
public struct PrePopulateEntityDataEvent : INotification
{
    public IObjectEntity Entity { get; }

    public PrePopulateEntityDataEvent(IObjectEntity entity)
    {
        Entity = entity;
    }
}

public interface PrePopulateEntityDataEventObserver
    : ArgumentObserver<PrePopulateEntityDataEvent> { }
