namespace EventHorizon.Zone.System.Client.Scripts.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupClientScriptsSystemCommandHandler
        : IRequestHandler<OnStartupSetupClientScriptsSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupClientScriptsSystemCommandHandler(
            ILogger<OnStartupSetupClientScriptsSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupClientScriptsSystemCommand request, 
            CancellationToken cancellationToken
        )
        {
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    _serverInfo.ClientScriptsPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    _serverInfo.ClientScriptsPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        _serverInfo.ClientScriptsPath
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
