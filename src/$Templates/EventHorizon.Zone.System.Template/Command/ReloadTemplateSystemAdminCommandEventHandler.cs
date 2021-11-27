namespace EventHorizon.Zone.System.Template.Command;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.Template.ClientActions;
using EventHorizon.Zone.System.Template.Reload;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class ReloadTemplateSystemAdminCommandEventHandler
    : INotificationHandler<AdminCommandEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ReloadTemplateSystemAdminCommandEventHandler(
        ILogger<ReloadTemplateSystemAdminCommandEventHandler> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(
        AdminCommandEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (notification.Command.Command != "reload-system")
        {
            return;
        }

        _logger.LogInformation(
            "reload-system : {CommandHandler}",
            nameof(ReloadTemplateSystemAdminCommandEventHandler)
        );

        await _mediator.Send(
            new ReloadTemplateSystemCommand(),
            cancellationToken
        );

        await _mediator.Send(
            new RespondToAdminCommand(
                notification.ConnectionId,
                new StandardAdminCommandResponse(
                    notification.Command.Command,
                    notification.Command.RawCommand,
                    true,
                    "template_system_reloaded"
                )
            ),
            cancellationToken
        );

        await _mediator.Publish(
            AdminClientActionTemplateSystemReloadedToAllEvent.Create(),
            cancellationToken
        );
    }
}
