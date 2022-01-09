namespace EventHorizon.Zone.System.EntityModule.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class OnStartupSetupEntityModuleSystemCommandHandler
    : IRequestHandler<OnStartupSetupEntityModuleSystemCommand, OnServerStartupResult>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ServerInfo _serverInfo;

    public OnStartupSetupEntityModuleSystemCommandHandler(
        ILogger<OnStartupSetupEntityModuleSystemCommandHandler> logger,
        IMediator mediator,
        ServerInfo serverInfo
    )
    {
        _logger = logger;
        _mediator = mediator;
        _serverInfo = serverInfo;
    }

    public async Task<OnServerStartupResult> Handle(
        OnStartupSetupEntityModuleSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        await ValidateEntityModuleDirectories(
            cancellationToken
        );

        return new OnServerStartupResult(
            true
        );
    }

    private async Task ValidateEntityModuleDirectories(
        CancellationToken cancellationToken
    )
    {
        await CreateDirectory(
            Path.Combine(
                _serverInfo.ClientPath,
                "Modules",
                "Base"
            ),
            cancellationToken
        );
        await CreateDirectory(
            Path.Combine(
                _serverInfo.ClientPath,
                "Modules",
                "Player"
            ),
            cancellationToken
        );
    }

    private async Task CreateDirectory(
        string directory,
        CancellationToken cancellationToken
    )
    {
        // Validate Directory Exists
        if (!await _mediator.Send(
            new DoesDirectoryExist(
                directory
            ),
            cancellationToken
        ))
        {
            _logger.LogWarning(
                "Directory '{DirectoryFullName}' was not found, creating...",
                directory
            );
            await _mediator.Send(
                new CreateDirectory(
                    directory
                ),
                cancellationToken
            );
        }
    }
}
