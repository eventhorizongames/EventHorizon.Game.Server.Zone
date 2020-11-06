namespace EventHorizon.Zone.System.Gui.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupGuiSystemCommandHandler
        : IRequestHandler<OnStartupSetupGuiSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupGuiSystemCommandHandler(
            ILogger<OnStartupSetupGuiSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupGuiSystemCommand request, 
            CancellationToken cancellationToken
        )
        {
            var clientGuiPath = Path.Combine(
                _serverInfo.ClientPath,
                "Gui"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    clientGuiPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    clientGuiPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        clientGuiPath
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
