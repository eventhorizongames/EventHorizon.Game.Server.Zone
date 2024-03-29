﻿namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class OnStartupSetupCombatSkillPluginCommandHandler
    : IRequestHandler<OnStartupSetupCombatSkillPluginCommand, OnServerStartupResult>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ServerInfo _serverInfo;

    public OnStartupSetupCombatSkillPluginCommandHandler(
        ILogger<OnStartupSetupCombatSkillPluginCommandHandler> logger,
        IMediator mediator,
        ServerInfo serverInfo
    )
    {
        _logger = logger;
        _mediator = mediator;
        _serverInfo = serverInfo;
    }

    public async Task<OnServerStartupResult> Handle(
        OnStartupSetupCombatSkillPluginCommand request,
        CancellationToken cancellationToken
    )
    {
        await ValidateDirectory(
            Path.Combine(
                _serverInfo.ClientPath,
                "Skills"
            ),
            cancellationToken
        );
        await ValidateDirectory(
            Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Effects"
            ),
            cancellationToken
        );
        await ValidateDirectory(
            Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Validators"
            ),
            cancellationToken
        );

        return new OnServerStartupResult(
            true
        );
    }

    private async Task ValidateDirectory(
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
