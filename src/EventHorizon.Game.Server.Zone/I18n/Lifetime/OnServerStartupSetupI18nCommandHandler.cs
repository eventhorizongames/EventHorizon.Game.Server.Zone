namespace EventHorizon.Game.Server.Zone.I18n.Lifetime
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnServerStartupSetupI18nCommandHandler
        : IRequestHandler<OnServerStartupSetupI18nCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnServerStartupSetupI18nCommandHandler(
            ILogger<OnServerStartupSetupI18nCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnServerStartupSetupI18nCommand request,
            CancellationToken cancellationToken
        )
        {
            await ValidateCommandFile(
                cancellationToken
            );
            await ValidateScriptsFiles(
                cancellationToken
            );

            return new OnServerStartupResult(
                true
            );
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
                "ReloadI18n.json",
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

        private async Task ValidateScriptsFiles(
            CancellationToken cancellationToken
        )
        {
            var scriptsPath = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Admin",
                "I18n"
            );
            var scriptFileList = new List<string>
            {
                "ReloadI18nSystem.csx"
            };

            foreach (var scriptFile in scriptFileList)
            {
                await WriteResourceFile(
                    "App_Data.Server.Scripts.Admin.I18n",
                    scriptFile,
                    scriptsPath,
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
                    "EventHorizon.Game.Server.Zone.I18n",
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
                    "Failed to create Startup File: {FileName} | ErrorCode: {ErrorCode}",
                    resourceFile,
                    result.ErrorCode
                );
            }
        }
    }
}
