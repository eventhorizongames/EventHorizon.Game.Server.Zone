namespace EventHorizon.Zone.System.Template.Background;

using EventHorizon.Observer.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TemplateBackgroundEventObserverHandler
    : INotificationHandler<TemplateBackgroundEvent>
{
    private readonly ObserverState _observer;

    public TemplateBackgroundEventObserverHandler(
        ObserverState observer
    )
    {
        _observer = observer;
    }

    public Task Handle(
        TemplateBackgroundEvent notification,
        CancellationToken cancellationToken
    ) => _observer.Trigger<TemplateBackgroundEventObserver, TemplateBackgroundEvent>(
        notification,
        cancellationToken
    );
}
