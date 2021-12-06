namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Command;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class ReloadAgentSystemBehaviorPluginAdminCommandEventHandler
    : INotificationHandler<AdminCommandEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ReloadAgentSystemBehaviorPluginAdminCommandEventHandler(
        ILogger<ReloadAgentSystemBehaviorPluginAdminCommandEventHandler> logger,
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
            nameof(ReloadAgentSystemBehaviorPluginAdminCommandEventHandler)
        );

        await _mediator.Send(
            new LoadAgentBehaviorSystem(),
            cancellationToken
        );

        await _mediator.Send(
            new RespondToAdminCommand(
                notification.ConnectionId,
                new StandardAdminCommandResponse(
                    notification.Command.Command,
                    notification.Command.RawCommand,
                    true,
                    "agent_system_behavior_plugin_reloaded"
                )
            ),
            cancellationToken
        );
    }
}
