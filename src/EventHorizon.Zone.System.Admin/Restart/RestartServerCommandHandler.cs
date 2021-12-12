namespace EventHorizon.Zone.System.Admin.Restart;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class RestartServerCommandHandler
    : IRequestHandler<RestartServerCommand, StandardCommandResult>
{
    private readonly ILogger<RestartServerCommandHandler> _logger;
    private readonly ISender _sender;
    private readonly IPublisher _publisher;

    public RestartServerCommandHandler(
        ILogger<RestartServerCommandHandler> logger,
        ISender sender,
        IPublisher publisher)
    {
        _logger = logger;
        _sender = sender;
        _publisher = publisher;
    }

    public async Task<StandardCommandResult> Handle(
        RestartServerCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await _sender.Send(
                new RunServerStartupCommand(),
                cancellationToken
            );
            await _publisher.Publish(
                new AdminCommandEvent(
                    BuildAdminCommand.FromString(
                        "reload-admin"
                    ),
                    "reload-admin"
                ),
                cancellationToken
            );
            await _publisher.Publish(
                new AdminCommandEvent(
                    BuildAdminCommand.FromString(
                        "reload-system"
                    ),
                    "reload-system"
                ),
                cancellationToken
            );
            await _sender.Send(
                new FinishServerStartCommand(),
                cancellationToken
            );

            return new();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to Restart Server."
            );

            return "SERVER_RESTART_ERROR";
        }
    }
}
