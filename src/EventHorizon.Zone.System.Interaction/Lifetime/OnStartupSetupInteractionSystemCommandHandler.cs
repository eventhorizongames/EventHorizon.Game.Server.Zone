namespace EventHorizon.Zone.System.Interaction.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnStartupSetupInteractionSystemCommandHandler
        : IRequestHandler<OnStartupSetupInteractionSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupInteractionSystemCommandHandler(
            ILogger<OnStartupSetupInteractionSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupInteractionSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var interactionPath = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Interaction"
            );
            // Validate Directory Exists
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    interactionPath
                ),
                cancellationToken
            ))
            {
                _logger.LogWarning(
                    "Directory '{DirectoryFullName}' was not found, creating...",
                    interactionPath
                );
                await _mediator.Send(
                    new CreateDirectory(
                        interactionPath
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
