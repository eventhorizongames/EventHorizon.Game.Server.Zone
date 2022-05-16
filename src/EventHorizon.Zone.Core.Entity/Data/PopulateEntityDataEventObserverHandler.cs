namespace EventHorizon.Zone.Core.Events.Entity.Data;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Observer.State;

using MediatR;

public class PopulateEntityDataEventObserverHandler : INotificationHandler<PopulateEntityDataEvent>
{
    private readonly ObserverState _observer;

    public PopulateEntityDataEventObserverHandler(ObserverState observer)
    {
        _observer = observer;
    }

    public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken) =>
        _observer.Trigger<PopulateEntityDataEventObserver, PopulateEntityDataEvent>(
            notification,
            cancellationToken
        );
}
