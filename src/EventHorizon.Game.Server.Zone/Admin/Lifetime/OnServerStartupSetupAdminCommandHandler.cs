namespace EventHorizon.Game.Server.Zone.Admin.Lifetime
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class OnServerStartupSetupAdminCommandHandler
        : IRequestHandler<OnServerStartupSetupAdminCommand, OnServerStartupResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public OnServerStartupSetupAdminCommandHandler(
            ILogger<OnServerStartupSetupAdminCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<OnServerStartupResult> Handle(
            OnServerStartupSetupAdminCommand request,
            CancellationToken cancellationToken
        )
        {
            var directoryList = new List<string>
            {
                _serverInfo.ServerPath,
                _serverInfo.AdminPath,
                _serverInfo.I18nPath,
                _serverInfo.ClientPath,
                Path.Combine(
                    _serverInfo.AppDataPath,
                    "Agent",
                    "Reload"
                ),
                _serverInfo.CoreMapPath,
            };
            foreach (var directoryFullName in directoryList)
            {
                // Validate Directory Exists
                if (!await _mediator.Send(
                    new DoesDirectoryExist(
                        directoryFullName
                    ),
                    cancellationToken
                ))
                {
                    _logger.LogWarning(
                        "Required Admin Directory of '{DirectoryFullName}' was not found, creating...",
                        directoryFullName
                    );
                    await _mediator.Send(
                        new CreateDirectory(
                            directoryFullName
                        ),
                        cancellationToken
                    );
                }
            }

            return new OnServerStartupResult(
                true
            );
        }
    }
}
