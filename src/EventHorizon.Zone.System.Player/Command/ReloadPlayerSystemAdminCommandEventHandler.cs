namespace EventHorizon.Zone.System.Player.Command
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Player.Reload;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class ReloadPlayerSystemAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadPlayerSystemAdminCommandEventHandler(
            ILogger<ReloadPlayerSystemAdminCommandEventHandler> logger,
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
                nameof(ReloadPlayerSystemAdminCommandEventHandler)
            );

            await _mediator.Send(
                new ReloadPlayerSystemCommand(),
                cancellationToken
            );

            await _mediator.Send(
                new RespondToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "player_system_reloaded"
                    )
                ),
                cancellationToken
            );
        }
    }
}
