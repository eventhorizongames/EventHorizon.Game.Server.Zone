namespace EventHorizon.Zone.Core.Map.Lifetime
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
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
            await ValidateDefaultState(
                cancellationToken
            );
            await ValidateCommandFile(
                cancellationToken
            );
            await ValidateScriptFiles(
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
        }

        private async Task ValidateDefaultState(
            CancellationToken cancellationToken
        )
        {
            // Validate App_Data/Map details exists
            var stateFile = Path.Combine(
                _serverInfo.CoreMapPath,
                "Map.state.json"
            );
            if (!await _mediator.Send(
                new DoesFileExist(
                    stateFile
                ),
                cancellationToken
            ))
            {
                // Create default Map Settings
                _logger.LogWarning(
                    "Zone Map Details Not Found, creating Default. {ZoneMapDetailsFilePath}",
                    stateFile
                );
                await _fileSaver.SaveToFile(
                    Path.GetDirectoryName(stateFile),
                    Path.GetFileName(stateFile),
                    DefaultMapSettings.DEFAULT_MAP
                );
            }
        }

        private async Task ValidateCommandFile(
            CancellationToken cancellationToken
        )
        {
            var commandsPath = Path.Combine(
                _serverInfo.AdminPath,
                "Commands"
            );
            var commandFileList = new List<string>
            {
                "ReloadCoreMap.json",
                "ReloadCoreMap_cmd.json",
            };

            foreach (var commandFile in commandFileList)
            {
                await WriteResourceFile(
                    "App_Data.Admin.Commands",
                    commandFile,
                    commandsPath,
                    cancellationToken
                );
            }
        }

        private async Task ValidateScriptFiles(
            CancellationToken cancellationToken
        )
        {
            var scriptsPath = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Admin",
                "Map"
            );
            var reloadScriptFile = "ReloadCoreMap.csx";

            await WriteResourceFile(
                "App_Data.Server.Scripts.Admin.Map",
                reloadScriptFile,
                scriptsPath,
                cancellationToken
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
                    "EventHorizon.Zone.Core.Map",
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
