namespace EventHorizon.Zone.System.Template.Lifetime;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Template.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class TemplateStartupOnServerFinishedStartingEventHandler
    : INotificationHandler<ServerFinishedStartingEvent>
{
    private readonly ISender _sender;

    public TemplateStartupOnServerFinishedStartingEventHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public async Task Handle(
        ServerFinishedStartingEvent notification,
        CancellationToken cancellationToken
    )
    {
        await _sender.Send(
            new LoadTemplateSystemCommand(),
            cancellationToken
        );
    }
}
