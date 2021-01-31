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
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _lifeTime;
        private readonly IMediator _mediator;

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime lifeTime,
            IMediator mediator
        )
        {
            _logger = logger;
            _lifeTime = lifeTime;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken
        )
        {
            _logger.LogInformation(
                "Worker Started Running at: {StartTime}",
                DateTimeOffset.Now
            );
            await _mediator.Send(
                new CompileClientScriptsCommand(),
                stoppingToken
            );
            await Task.Delay(
                5000,
                stoppingToken
            );
            _lifeTime.StopApplication();
        }
    }
}
