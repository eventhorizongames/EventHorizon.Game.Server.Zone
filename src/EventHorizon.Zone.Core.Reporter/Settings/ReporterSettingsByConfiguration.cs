namespace EventHorizon.Zone.Core.Reporter.Settings
{
    using EventHorizon.Zone.Core.Reporter.Model;
    using Microsoft.Extensions.Configuration;

    public class ReporterSettingsByConfiguration : ReporterSettings
    {
        private const string CONFIGURATION_SECTION = "Reporter";

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
                        RegisterChangeCallback();
                    },
                    token
                );
        }

        public class ConfigurationElasticsearchReporterSettings : ReporterSettings.ElasticsearchReporterSettings
        {
            public bool IsEnabled { get; set; }
            public string Url { get; set; }
        }
    }
}