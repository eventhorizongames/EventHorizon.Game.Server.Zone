namespace EventHorizon.Zone.System.EntityModule.Command
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.EntityModule.Load;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ReloadEntityModuleSystemAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadEntityModuleSystemAdminCommandEventHandler(
            ILogger<ReloadEntityModuleSystemAdminCommandEventHandler> logger,
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
                nameof(ReloadEntityModuleSystemAdminCommandEventHandler)
            );

            await _mediator.Publish(
                new LoadEntityModuleSystemCommand()
            );

            await _mediator.Send(
                new RespondToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "entity_module_system_reloaded"
                    )
                )
            );
        }
    }
}
