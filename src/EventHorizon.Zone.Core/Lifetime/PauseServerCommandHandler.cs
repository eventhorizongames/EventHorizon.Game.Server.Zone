namespace EventHorizon.Zone.Core.Lifetime.NewFolder;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Lifetime.State;
using EventHorizon.Zone.Core.Model.Command;

using MediatR;

using Microsoft.Extensions.Logging;

public class PauseServerCommandHandler
    : IRequestHandler<PauseServerCommand, StandardCommandResult>
{
    private readonly ILogger _logger;
    private readonly ServerLifetimeState _serverLifetimeState;

    public PauseServerCommandHandler(
        ILogger<FinishServerStartCommandHandler> logger,
        ServerLifetimeState serverLifetimeState
    )
    {
        _logger = logger;
        _serverLifetimeState = serverLifetimeState;
    }

    public Task<StandardCommandResult> Handle(
        PauseServerCommand request,
        CancellationToken cancellationToken
)
    {
        _serverLifetimeState.SetServerStarted(
            false
        );

        _logger.LogInformation(
            "Server Paused"
        );

        return new StandardCommandResult()
            .FromResult();
    }
}
