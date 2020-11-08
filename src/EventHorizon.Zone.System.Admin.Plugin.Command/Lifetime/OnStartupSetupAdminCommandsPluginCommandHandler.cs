namespace EventHorizon.Zone.System.Admin.Plugin.Command.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupAdminCommandsPluginCommandHandler
        : IRequestHandler<OnStartupSetupAdminCommandsPluginCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupAdminCommandsPluginCommandHandler(
            ILogger<OnStartupSetupAdminCommandsPluginCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupAdminCommandsPluginCommand request,
            CancellationToken cancellationToken
        )
        {
            var commandsPath = Path.Combine(
                _serverInfo.AdminPath,
                "Commands"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    commandsPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    commandsPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        commandsPath
                    ),
                    cancellationToken
                );
            }

            return new OnServerStartupResult(
                true
            );
        }
    }
}
