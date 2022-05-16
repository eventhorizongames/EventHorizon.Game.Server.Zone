namespace EventHorizon.Zone.Core.Events.Entity.Data;

using EventHorizon.Observer.Model;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Observable;

using MediatR;

/// <summary>
/// The event sent after an Entity is created to fill Entity Data.
/// </summary>
[ObservableEvent]
public struct PopulateEntityDataEvent : INotification
{
    public IObjectEntity Entity { get; set; }

    public PopulateEntityDataEvent(IObjectEntity entity)
    {
        Entity = entity;
    }
}

public interface PopulateEntityDataEventObserver : ArgumentObserver<PopulateEntityDataEvent> { }
