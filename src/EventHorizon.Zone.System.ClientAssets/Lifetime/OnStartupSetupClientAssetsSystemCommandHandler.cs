namespace EventHorizon.Zone.System.ClientAssets.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupClientAssetsSystemCommandHandler
        : IRequestHandler<OnStartupSetupClientAssetsSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupClientAssetsSystemCommandHandler(
            ILogger<OnStartupSetupClientAssetsSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupClientAssetsSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            await ValidateClientAssetsDirectory(
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
        }

        private async Task ValidateClientAssetsDirectory(
            CancellationToken cancellationToken
        )
        {
            var particlePath = Path.Combine(
                _serverInfo.ClientPath,
                "Assets"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    particlePath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    particlePath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        particlePath
                    ),
                    cancellationToken
                );
            }
        }
    }
}
