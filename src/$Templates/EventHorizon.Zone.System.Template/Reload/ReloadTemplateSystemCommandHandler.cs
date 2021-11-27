namespace EventHorizon.Zone.System.Template.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Template.ClientActions;
using EventHorizon.Zone.System.Template.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadTemplateSystemCommandHandler
    : IRequestHandler<ReloadTemplateSystemCommand, StandardCommandResult>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public ReloadTemplateSystemCommandHandler(
        ISender sender,
        IPublisher publisher
    )
    {
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadTemplateSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(
            new LoadTemplateSystemCommand(),
            cancellationToken
        );

        if (result
            && result.WasUpdated
        )
        {
            await _publisher.Publish(
                ClientActionTemplateSystemReloadedToAllEvent.Create(),
                cancellationToken
            );
        }

        return new();
    }
}
