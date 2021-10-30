namespace EventHorizon.Zone.Core.Entity.Command
{
    using EventHorizon.Zone.Core.Events.Entity.Reload;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class ReloadEntityCoreAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadEntityCoreAdminCommandEventHandler(
            ILogger<ReloadEntityCoreAdminCommandEventHandler> logger,
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
                nameof(ReloadEntityCoreAdminCommandEventHandler)
            );

            await _mediator.Send(
                new ReloadEntityCoreCommand(),
                cancellationToken
            );

            await _mediator.Send(
                new RespondToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "entity_core_reloaded"
                    )
                ),
                cancellationToken
            );
        }
    }
}
