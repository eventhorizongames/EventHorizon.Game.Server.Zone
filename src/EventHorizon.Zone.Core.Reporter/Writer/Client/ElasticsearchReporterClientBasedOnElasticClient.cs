namespace EventHorizon.Zone.Core.Reporter.Writer.Client;

using System;
using System.Threading;
using System.Threading.Tasks;

using Elasticsearch.Net;

using EventHorizon.Zone.Core.Reporter.Model;

using Microsoft.Extensions.Logging;

public class ElasticsearchReporterClientBasedOnElasticClient
    : ElasticsearchReporterClient,
    ElasticsearchReporterClientStartup
{
    private ElasticLowLevelClient? _client;

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
        if (!IsConnected
            || _client == null
        )
        {
            return false;
        }

        await _client.BulkAsync<StringResponse>(
            PostData.MultiJson(body)
        );

        return true;
    }

    public void StartUp()
    {
        if (!_settings.Elasticsearch.IsEnabled)
        {
            _client = null;
            IsConnected = false;
            return;
        }
        if (string.IsNullOrEmpty(
            _settings.Elasticsearch.Uri
        ))
        {
            _client = null;
            IsConnected = false;
            _logger.LogError(
                "Elasticsearch.Uri is not configured. {@Settings}",
                _settings.Elasticsearch
            );
            return;
        }
        if (_client == null)
        {
            var settings = new ConnectionConfiguration(
                new Uri(
                    _settings.Elasticsearch.Uri
                )
            ).RequestTimeout(
                TimeSpan.FromMinutes(2)
            );
            settings.BasicAuthentication(
                _settings.Elasticsearch.Username,
                _settings.Elasticsearch.Password
            );

            _client = new ElasticLowLevelClient(
                settings
            );
            var response = _client.Ping<PingResponse>();
            if (!response.Success)
            {
                _client = null;
                IsConnected = false;
                _logger.LogError(
                    "Did not receive successful Ping response from Elasticsearch.",
                    response
                );
                return;
            }

            // Build the Index, we need to make sure that it has date_nanos
            _client.Indices.Create<StringResponse>(
                // TODO: Reporter - This will need an index created specifically for the PlatformId.
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

    public class PingResponse
        : ElasticsearchResponse<VoidResponse>
    {

    }
}
