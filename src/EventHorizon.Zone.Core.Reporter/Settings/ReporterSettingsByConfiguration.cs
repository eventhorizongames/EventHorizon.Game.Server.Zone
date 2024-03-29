namespace EventHorizon.Zone.Core.Reporter.Settings;

using EventHorizon.Zone.Core.Reporter.Model;

using Microsoft.Extensions.Configuration;

public class ReporterSettingsByConfiguration : ReporterSettings
{
    private const string CONFIGURATION_SECTION = "Reporter";
    private const string ELASITCSEARCH_CONFIGURATION_SECTION = "Elasticsearch";

    private readonly IConfiguration _configuration;

    public bool IsEnabled { get; set; }
    public bool IsWriteToFileEnabled { get; set; }
    public ReporterSettings.ElasticsearchReporterSettings Elasticsearch { get; set; } = new ConfigurationElasticsearchReporterSettings();

    public ReporterSettingsByConfiguration(
        IConfiguration configuration
    )
    {
        _configuration = configuration;
        _configuration.GetSection(
            CONFIGURATION_SECTION
        ).Bind(this);
        Elasticsearch = new ConfigurationElasticsearchReporterSettings
        {
            IsEnabled = Elasticsearch.IsEnabled,
            Uri = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Uri"],
            Username = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Username"],
            Password = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Password"],
        };
        RegisterChangeCallback();
    }

    private void RegisterChangeCallback()
    {
        // Trigger is enabled check on configuration changed.
        var token = _configuration.GetReloadToken();
        token.RegisterChangeCallback(
                _ =>
                {
                    _configuration.GetSection(
                        CONFIGURATION_SECTION
                    ).Bind(this);
                    Elasticsearch = new ConfigurationElasticsearchReporterSettings
                    {
                        IsEnabled = Elasticsearch.IsEnabled,
                        Uri = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Uri"],
                        Username = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Username"],
                        Password = _configuration[$"{ELASITCSEARCH_CONFIGURATION_SECTION}:Password"],
                    };
                    RegisterChangeCallback();
                },
                token
            );
    }

    public class ConfigurationElasticsearchReporterSettings
        : ReporterSettings.ElasticsearchReporterSettings
    {
        public bool IsEnabled { get; set; }
        public string Uri { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
