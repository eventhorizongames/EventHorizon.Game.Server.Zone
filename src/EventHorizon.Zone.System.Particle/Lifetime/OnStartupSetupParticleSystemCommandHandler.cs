namespace EventHorizon.Zone.System.Particle.Lifetime
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnStartupSetupParticleSystemCommandHandler
        : IRequestHandler<OnStartupSetupParticleSystemCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnStartupSetupParticleSystemCommandHandler(
            ILogger<OnStartupSetupParticleSystemCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnStartupSetupParticleSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            await ValidateParticleSettingsFiles(
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
        }

        private async Task ValidateParticleSettingsFiles(
            CancellationToken cancellationToken
        )
        {
            var particlePath = Path.Combine(
                _serverInfo.ClientPath,
                "Particle"
            );
            var particleFileList = new List<string>
            {
                "Flame.json",
                "SelectedCompanionIndicator.json",
                "SelectedIndicator.json",
            };

            foreach (var file in particleFileList)
            {
                await WriteResourceFile(
                    "App_Data.Client.Particle",
                    file,
                    particlePath,
                    cancellationToken
                );
            }
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
                    "EventHorizon.Zone.System.Particle",
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
                    "Failed to create Startup File: {FileName}",
                    resourceFile
                );
            }
        }
    }
}
