namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
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

            // Check/Create Default Actor Behavior Tree
            await CheckCreateDefaultBehaviorTree(
                cancellationToken
            );
            // Check/Create Default Actor Behavior Script
            await CheckCreateDefaultBehaviorScript(
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

        private async Task CheckCreateDefaultBehaviorTree(
            CancellationToken cancellationToken
        )
        {
            var defaultShapeFile = Path.Combine(
                _serverInfo.SystemPath,
                "Behaviors",
                "$DEFAULT$SHAPE.json"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesFileExist(
                    defaultShapeFile
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Default Behavior Shape was not found, creating..."
                );
                await _mediator.Send(
                    new CreateFile(
                        defaultShapeFile
                    ),
                    cancellationToken
                );
                await _mediator.Send(
                    new WriteAllTextToFile(
                        defaultShapeFile,
                        BehaviorDefaultSettings.DEFAULT_SHAPE
                    ),
                    cancellationToken
                );
            }
        }

        private async Task CheckCreateDefaultBehaviorScript(
            CancellationToken cancellationToken
        )
        {
            var defaultScriptFile = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "System",
                "Behaviors",
                "$DEFAULT$SCRIPT.csx"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesFileExist(
                    defaultScriptFile
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Default Behavior Script was not found, creating..."
                );
                await _mediator.Send(
                    new CreateFile(
                        defaultScriptFile
                    ),
                    cancellationToken
                );
                await _mediator.Send(
                    new WriteAllTextToFile(
                        defaultScriptFile,
                        BehaviorDefaultSettings.DEFAULT_SCRIPT
                    ),
                    cancellationToken
                );
            }
        }
    }
}
