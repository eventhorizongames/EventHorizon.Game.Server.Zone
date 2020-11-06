namespace EventHorizon.Zone.System.ClientEntities.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupClientEntitiesSystemCommandHandler
        : IRequestHandler<OnStartupSetupClientEntitiesSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupClientEntitiesSystemCommandHandler(
            ILogger<OnStartupSetupClientEntitiesSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupClientEntitiesSystemCommand request, 
            CancellationToken cancellationToken
        )
        {
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    _serverInfo.ClientEntityPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    _serverInfo.ClientEntityPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        _serverInfo.ClientEntityPath
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
