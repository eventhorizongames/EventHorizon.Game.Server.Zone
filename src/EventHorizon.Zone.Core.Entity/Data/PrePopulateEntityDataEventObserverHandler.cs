namespace EventHorizon.Zone.Core.Events.Entity.Data;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Observer.State;

using MediatR;

public class PrePopulateEntityDataEventObserverHandler
    : INotificationHandler<PrePopulateEntityDataEvent>
{
    private readonly ObserverState _observer;

    public PrePopulateEntityDataEventObserverHandler(ObserverState observer)
    {
        _observer = observer;
    }

    public Task Handle(
        PrePopulateEntityDataEvent notification,
        CancellationToken cancellationToken
    ) =>
        _observer.Trigger<PrePopulateEntityDataEventObserver, PrePopulateEntityDataEvent>(
            notification,
            cancellationToken
        );
}
