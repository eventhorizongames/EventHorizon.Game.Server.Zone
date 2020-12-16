namespace EventHorizon.Zone.System.Agent.Command
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Agent.Reload;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ReloadAgentSystemAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReloadAgentSystemAdminCommandEventHandler(
            ILogger<ReloadAgentSystemAdminCommandEventHandler> logger,
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
                nameof(ReloadAgentSystemAdminCommandEventHandler)
            );

            var result = await _mediator.Send(
                new ReloadAgentSystemCommand(),
                cancellationToken
            );

            if (result.Success)
            {
                await _mediator.Send(
                    new RespondToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            notification.Command.Command,
                            notification.Command.RawCommand,
                            true,
                            "agent_system_reloaded"
                        )
                    ),
                    cancellationToken
                );
            }
        }
    }
}
