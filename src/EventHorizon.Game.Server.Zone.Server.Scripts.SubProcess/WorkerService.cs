namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.Compile;

using MediatR;

using Microsoft.Extensions.Hosting;

public class WorkerService
    : BackgroundService
{
    private readonly IHostApplicationLifetime _lifeTime;
    private readonly IMediator _mediator;

    public WorkerService(
        IHostApplicationLifetime lifeTime,
        IMediator mediator
    )
    {
        _lifeTime = lifeTime;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        await _mediator.Send(
            new CompileServerScriptsCommand(),
            stoppingToken
        );
        _lifeTime.StopApplication();
    }
}
