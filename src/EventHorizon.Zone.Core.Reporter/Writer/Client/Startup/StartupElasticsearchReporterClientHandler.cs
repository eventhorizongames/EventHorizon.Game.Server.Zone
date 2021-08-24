namespace EventHorizon.Zone.Core.Reporter.Writer.Client.Startup
{
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    public class StartupElasticsearchReporterClientHandler
        : IRequestHandler<StartupElasticsearchReporterClient>
    {
        private readonly ElasticsearchReporterClientStartup _client;

        public StartupElasticsearchReporterClientHandler(
            ElasticsearchReporterClientStartup client
        )
        {
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
