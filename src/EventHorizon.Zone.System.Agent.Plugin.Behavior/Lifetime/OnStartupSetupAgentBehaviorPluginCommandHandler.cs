namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupAgentBehaviorPluginCommandHandler
        : IRequestHandler<OnStartupSetupAgentBehaviorPluginCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupAgentBehaviorPluginCommandHandler(
            ILogger<OnStartupSetupAgentBehaviorPluginCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupAgentBehaviorPluginCommand request,
            CancellationToken cancellationToken
        )
        {
            // Check/Create Behavior Scripts Directory
            await CheckCreateDirectory(
                Path.Combine(
                    _serverInfo.ServerScriptsPath,
                    "Behavior"
                ),
                cancellationToken
            );
            // Check/Create Behavior Tree Shapes Directory
            await CheckCreateDirectory(
                Path.Combine(
                    _serverInfo.ServerPath,
                    "Behaviors"
                ),
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
        }

        private async Task CheckCreateDirectory(
            string directory,
            CancellationToken cancellationToken
        )
        {
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    directory
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    directory
                );
                await _mediator.Send(
                    new CreateDirectory(
                        directory
                    ),
                    cancellationToken
                );
            }
        }
    }
}
