namespace EventHorizon.Zone.Core.Reporter.Writer
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Writer.Client;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class WriteReportToElasticsearchHandler : IRequestHandler<WriteReportToElasticsearch>
    {
        private readonly ILogger _logger;
        private readonly ElasticsearchReporterClient _client;

        public WriteReportToElasticsearchHandler(
            ILogger<WriteReportToElasticsearchHandler> logger,
            ElasticsearchReporterClient client
        )
        {
            _logger = logger;
            _client = client;
        }

        public async Task Handle(
            WriteReportToElasticsearch request,
            CancellationToken cancellationToken
        )
        {
            if (!_client.IsConnected)
            {
                _logger.LogWarning(
                    "Skipping Write of Report to Elasticsearch. {ErrorCode} \n | Report: {@Report}",
                    "elasticsearch_is_not_connected",
                    request.Report
                );
                return;
            }

            var report = request.Report;
            var reportItemIndexes = new object[report.ItemList.Count * 2];
            for (int i = 0; i < report.ItemList.Count; i++)
            {
                var reportItem = report.ItemList.ElementAt(i);
                reportItemIndexes[i * 2] = new
                {
                    index = new
                    {
                        _index = "report",
                        _type = "_doc",
                        _id = Guid.NewGuid().ToString(),
                    }
                };
                reportItemIndexes[(i * 2) + 1] = new ReportIndexItem(request.Report, reportItem);
            }

            await _client.BulkAsync(reportItemIndexes, cancellationToken);
        }
    }

    public class ReportIndexItem
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string CorrelationId { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ReportIndexItem(Report report, ReportItem reportItem)
        {
            Id = report.Id;
            Timestamp = reportItem.Timestamp;
            CorrelationId = reportItem.CorrelationId;
            Message = reportItem.Message;
            Data = reportItem.Data;
        }
    }
}
