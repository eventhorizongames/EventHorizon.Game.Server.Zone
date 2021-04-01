namespace EventHorizon.Zone.System.Wizard.Command
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Wizard.Clear;
    using EventHorizon.Zone.System.Wizard.Load;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ReloadWizardSystemAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadWizardSystemAdminCommandEventHandler(
            ILogger<ReloadWizardSystemAdminCommandEventHandler> logger,
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
                nameof(ReloadWizardSystemAdminCommandEventHandler)
            );

            await _mediator.Send(
                new ClearWizardListCommand(),
                cancellationToken
            );

            var loadSystemsResult = await _mediator.Send(
                new LoadSystemsWizardListCommand(),
                cancellationToken
            );

            var loadCustomResult = await _mediator.Send(
                new LoadWizardListCommand(),
                cancellationToken
            );

            if (!loadSystemsResult 
                || !loadCustomResult
            )
            {
                await _mediator.Send(
                    new RespondToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            notification.Command.Command,
                            notification.Command.RawCommand,
                            false,
                            "wizard_system_not_reloaded"
                        )
                    ),
                    cancellationToken
                );
                return;
            }

            await _mediator.Send(
                new SendAdminCommandResponseToAllCommand(
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "wizard_system_reloaded"
                    )
                ),
                cancellationToken
            );
        }
    }
}
