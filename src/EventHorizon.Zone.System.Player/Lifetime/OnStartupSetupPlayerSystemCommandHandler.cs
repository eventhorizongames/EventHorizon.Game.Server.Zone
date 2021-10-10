namespace EventHorizon.Zone.System.Player.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Player.Model;

    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnStartupSetupPlayerSystemCommandHandler
        : IRequestHandler<OnStartupSetupPlayerSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupPlayerSystemCommandHandler(
            ILogger<OnStartupSetupPlayerSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupPlayerSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var playerPath = Path.Combine(
                _serverInfo.AppDataPath,
                PlayerSystemConstants.PlayerAppDataPath
            );

            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    playerPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    playerPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        playerPath
                    ),
                    cancellationToken
                );
            }

            await WriteResourceFile(
                "App_Data.Player",
                PlayerSystemConstants.PlayerConfigurationFileName,
                playerPath,
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
        }

        private async Task WriteResourceFile(
            string resourcePath,
            string resourceFile,
            string saveDirectory,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new WriteResourceToFile(
                    Assembly.GetExecutingAssembly(),
                    "EventHorizon.Zone.System.Player",
                    resourcePath,
                    resourceFile,
                    Path.Combine(
                        saveDirectory,
                        resourceFile
                    )
                ),
                cancellationToken
            );
            if (!result.Success
                && result.ErrorCode != "file_already_exists"
            )
            {
                _logger.LogWarning(
                    "Failed to create Startup File: {FileName} | ErrorCode: {ErrorCode}",
                    resourceFile,
                    result.ErrorCode
                );
            }
        }
    }
}
