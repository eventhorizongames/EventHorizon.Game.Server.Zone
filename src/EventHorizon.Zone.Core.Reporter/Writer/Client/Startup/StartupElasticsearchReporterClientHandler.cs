namespace EventHorizon.Zone.Core.Reporter.Writer.Client.Startup
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class StartupElasticsearchReporterClientHandler : IRequestHandler<StartupElasticsearchReporterClient>
    {
        private readonly ILogger _logger;
        private readonly ElasticsearchReporterClientStartup _client;

        public StartupElasticsearchReporterClientHandler(
            ILogger<StartupElasticsearchReporterClientHandler> logger,
            ElasticsearchReporterClientStartup client
        )
        {
            _logger = logger;
            _client = client;
        }

        public Task<Unit> Handle(
            StartupElasticsearchReporterClient request,
            CancellationToken cancellationToken
        )
        {
            _client.StartUp();
            return Unit.Task;
        }
    }
}
