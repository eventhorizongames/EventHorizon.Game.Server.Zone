namespace EventHorizon.Zone.System.Admin.Reload
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Server.Scripts.Events.Load;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ReloadClientScriptsAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger<ReloadClientScriptsAdminCommandEventHandler> _logger;
        private readonly IMediator _mediator;

        public ReloadClientScriptsAdminCommandEventHandler(
            ILogger<ReloadClientScriptsAdminCommandEventHandler> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(
            AdminCommandEvent notification,
            CancellationToken cancellationToken)
        {
            if (notification.Command.Command != "reload-system")
            {
                return;
            }

            _logger.LogInformation(
                "reload-system : ReloadClientScriptsAdminCommandEventHandler"
            );

            await _mediator.Send(
                new LoadServerScriptsCommand()
            );
        }
    }
}
