namespace EventHorizon.Zone.Core.Reporter.Writer.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Elasticsearch.Net;
    using EventHorizon.Zone.Core.Reporter.Model;
    using Microsoft.Extensions.Logging;

    public class ElasticsearchReporterClientBasedOnElasticClient
        : ElasticsearchReporterClient, ElasticsearchReporterClientStartup
    {
        private ElasticLowLevelClient ElasticClient;

        private readonly ILogger _logger;
        private readonly ReporterSettings _settings;

        public bool IsConnected { get; private set; }

        public ElasticsearchReporterClientBasedOnElasticClient(
            ILogger<ElasticsearchReporterClientBasedOnElasticClient> logger,
            ReporterSettings reporterSettings
        )
        {
            _logger = logger;
            _settings = reporterSettings;
        }

        public async Task<bool> BulkAsync(
            object[] body,
            CancellationToken cancellationToken
        )
        {
            if (!IsConnected)
            {
                return false;
            }
            var asyncIndexResponse = await ElasticClient.BulkAsync<StringResponse>(
                PostData.MultiJson(body)
            );
            string responseString = asyncIndexResponse.Body;
            return true;
        }

        public void StartUp()
        {
            if (!_settings.Elasticsearch.IsEnabled)
            {
                ElasticClient = null;
                IsConnected = false;
                return;
            }
            if (string.IsNullOrEmpty(_settings.Elasticsearch.Url))
            {
                ElasticClient = null;
                IsConnected = false;
                _logger.LogError(
                    "Elasticsearch.Url is not configured. {@Settings}",
                    _settings.Elasticsearch
                );
                return;
            }
            if (ElasticClient == null)
            {
                var settings = new ConnectionConfiguration(
                    new Uri(
                        //_settings.Elasticsearch.Url,
                        "http://localhost:9200"
                    )
                ).RequestTimeout(
                    TimeSpan.FromMinutes(2)
                );

                ElasticClient = new ElasticLowLevelClient(
                    settings
                );
                var response = ElasticClient.Ping<PingResponse>();
                if (!response.Success)
                {
                    ElasticClient = null;
                    IsConnected = false;
                    _logger.LogError(
                        "Did not receive successful Ping response from Elasticsearch.",
                        response
                    );
                    return;
                }

                // Build the Index, we need to make sure that it has date_nanos
                ElasticClient.Indices.Create<StringResponse>(
                    "report",
                    PostData.Serializable(
                        new
                        {
                            mappings = new
                            {
                                properties = new
                                {
                                    Timestamp = new
                                    {
                                        type = "date_nanos"
                                    }
                                }
                            }
                        }
                    )
                );
            }
            IsConnected = true;
        }

        public class PingResponse : ElasticsearchResponse<VoidResponse>
        {

        }
    }
}
