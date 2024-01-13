namespace EventHorizon.Zone.Core.Reporter.Writer.Client.Check;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

public class CheckElasticsearchReporterClientConnectionHandler
    : INotificationHandler<CheckElasticsearchReporterClientConnection>
{
    private readonly ElasticsearchReporterClient _client;
    private readonly ElasticsearchReporterClientStartup _startup;

    public CheckElasticsearchReporterClientConnectionHandler(
        ElasticsearchReporterClient client,
        ElasticsearchReporterClientStartup startup
    )
    {
        _client = client;
        _startup = startup;
    }

    public Task Handle(
        CheckElasticsearchReporterClientConnection notification,
        CancellationToken cancellationToken
    )
    {
        if (!_client.IsConnected)
        {
            _startup.StartUp();
        }
        return Task.CompletedTask;
    }
}
