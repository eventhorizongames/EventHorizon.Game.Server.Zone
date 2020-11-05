namespace EventHorizon.Zone.Core.Map.Lifetime
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OnServerStartupSetupMapCommandHandler
        : IRequestHandler<OnServerStartupSetupMapCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileSaver _fileSaver;

        public OnServerStartupSetupMapCommandHandler(
            ILogger<OnServerStartupSetupMapCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
        }

        public async Task<OnServerStartupResult> Handle(
            OnServerStartupSetupMapCommand request,
            CancellationToken cancellationToken
        )
        {
            // Validate App_Data/Map details exists
            var stateFile = GetStateFileName();
            if (!await _mediator.Send(
                new DoesFileExist(
                    stateFile
                )
            ))
            {
                // Create default Map Settings
                _logger.LogWarning(
                    "Zone Map Details Not Found, creating Default. {ZoneMapDetailsFilePath}",
                    stateFile
                );
                await CreateDefaultState(
                    stateFile
                );
            }

            return new OnServerStartupResult(
                true
            );
        }

        private string GetStateFileName()
        {
            return Path.Combine(
                _serverInfo.CoreMapPath,
                "Map.state.json"
            );
        }

        private async Task CreateDefaultState(
            string stateFile
        )
        {
            await _fileSaver.SaveToFile(
                Path.GetDirectoryName(stateFile),
                Path.GetFileName(stateFile),
                DefaultMapSettings.DEFAULT_MAP
            );
        }
    }
}
