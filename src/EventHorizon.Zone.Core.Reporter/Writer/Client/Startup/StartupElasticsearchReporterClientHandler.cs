namespace EventHorizon.Zone.Core.Reporter.Writer.Client.Startup
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class StartupElasticsearchReporterClientHandler
        : IRequestHandler<StartupElasticsearchReporterClient>
    {
        private readonly ElasticsearchReporterClientStartup _client;

        public StartupElasticsearchReporterClientHandler(ElasticsearchReporterClientStartup client)
        {
            _client = client;
        }

        public Task Handle(
            StartupElasticsearchReporterClient request,
            CancellationToken cancellationToken
        )
        {
            _client.StartUp();

            return Task.CompletedTask;
        }
    }
}
