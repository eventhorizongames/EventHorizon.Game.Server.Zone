namespace EventHorizon.Zone.Core.Lifetime;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Lifetime.State;

using MediatR;

using Microsoft.Extensions.Logging;

public class FinishServerStartCommandHandler
: IRequestHandler<FinishServerStartCommand, bool>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ServerLifetimeState _serverLifetimeState;

    public FinishServerStartCommandHandler(
        ILogger<FinishServerStartCommandHandler> logger,
        IMediator mediator,
        ServerLifetimeState serverLifetimeState
    )
    {
        _logger = logger;
        _mediator = mediator;
        _serverLifetimeState = serverLifetimeState;
    }

    public async Task<bool> Handle(
        FinishServerStartCommand request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Publish(
            new ServerFinishedStartingEvent(),
            cancellationToken
        );

        _serverLifetimeState.SetServerStarted(
            true
        );

        _logger.LogInformation(
            "Server finished starting"
        );

        GC.Collect();

        return _serverLifetimeState.IsServerStarted();
    }
}
