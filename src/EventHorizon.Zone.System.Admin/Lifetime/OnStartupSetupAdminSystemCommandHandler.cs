namespace EventHorizon.Zone.System.Admin.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupAdminSystemCommandHandler
        : IRequestHandler<OnStartupSetupAdminSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupAdminSystemCommandHandler(
            ILogger<OnStartupSetupAdminSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupAdminSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var path = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Admin"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    path
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    path
                );
                await _mediator.Send(
                    new CreateDirectory(
                        path
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
