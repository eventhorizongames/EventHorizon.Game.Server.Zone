namespace EventHorizon.Game.Server.Zone.I18n.Command
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.I18n.Loader;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ReloadI18nAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadI18nAdminCommandEventHandler(
            ILogger<ReloadI18nAdminCommandEventHandler> logger,
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
                nameof(ReloadI18nAdminCommandEventHandler)
            );

            await _mediator.Send(
                new I18nLoadEvent(),
                cancellationToken
            );

            await _mediator.Send(
                new RespondToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "i18n_system_reloaded"
                    )
                ),
                cancellationToken
            );
        }
    }
}
