namespace EventHorizon.Zone.Core.Reporter.Writer.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ElasticsearchReporterClientStartup
    {
        void StartUp();
    }

    public interface ElasticsearchReporterClient
    {
        bool IsConnected { get; }
        Task<bool> BulkAsync(
            object[] body,
            CancellationToken cancellationToken
        );
    }
}
