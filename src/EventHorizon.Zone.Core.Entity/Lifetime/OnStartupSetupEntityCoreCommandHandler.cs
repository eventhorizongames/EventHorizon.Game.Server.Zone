namespace EventHorizon.Zone.Core.Entity.Lifetime
{
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Model;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnStartupSetupEntityCoreCommandHandler
        : IRequestHandler<OnStartupSetupEntityCoreCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupEntityCoreCommandHandler(
            ILogger<OnStartupSetupEntityCoreCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupEntityCoreCommand request,
            CancellationToken cancellationToken
        )
        {
            var playerPath = Path.Combine(
                _serverInfo.AppDataPath,
                EntityCoreConstants.EntityAppDataPath
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
                "App_Data.Entity",
                EntityCoreConstants.EntityConfigurationFileName,
                playerPath,
                cancellationToken
            );

            await WriteResourceFile(
                "App_Data.Entity",
                EntityCoreConstants.EntityDataFileName,
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
                    "EventHorizon.Zone.Core.Entity",
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
