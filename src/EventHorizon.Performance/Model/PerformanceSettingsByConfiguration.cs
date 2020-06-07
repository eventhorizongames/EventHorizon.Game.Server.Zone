namespace EventHorizon.Performance.Model
{
    using Microsoft.Extensions.Configuration;

    public class PerformanceSettingsByConfiguration : PerformanceSettings
    {
        private const string IS_ENABLED_KEY = "Performance:IsEnabled";

        private readonly IConfiguration _configuration;

        public bool IsEnabled { get; private set; }

        public PerformanceSettingsByConfiguration(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            IsEnabled = _configuration.GetValue<bool>(
                IS_ENABLED_KEY
            );
            RegisterChangeCallback();
        }

        private void RegisterChangeCallback()
        {
            // Trigger is enabled check on configuration changed.
            var token = _configuration.GetReloadToken();
            token.RegisterChangeCallback(
                    _ =>
                    {
                        IsEnabled = _configuration.GetValue<bool>(
                            IS_ENABLED_KEY
                        );
                        RegisterChangeCallback();
                    },
                    token
                );
        }

    }
}