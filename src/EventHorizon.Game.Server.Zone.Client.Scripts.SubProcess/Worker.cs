namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.Compile;

    using MediatR;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Worker
        : BackgroundService
    {
        private readonly IHostApplicationLifetime _lifeTime;
        private readonly IMediator _mediator;

        public Worker(
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
                new CompileClientScriptsCommand(),
                stoppingToken
            );
            _lifeTime.StopApplication();
        }
    }
}
