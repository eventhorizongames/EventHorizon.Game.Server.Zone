namespace EventHorizon.Zone.System.ServerModule.Lifetime
{
    using System;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnStartupSetupServerModuleSystemCommandHandler
        : IRequestHandler<OnStartupSetupServerModuleSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupServerModuleSystemCommandHandler(
            ILogger<OnStartupSetupServerModuleSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }


        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupServerModuleSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var serverModulePath = Path.Combine(
                _serverInfo.ClientPath,
                "ServerModule"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    serverModulePath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    serverModulePath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        serverModulePath
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
