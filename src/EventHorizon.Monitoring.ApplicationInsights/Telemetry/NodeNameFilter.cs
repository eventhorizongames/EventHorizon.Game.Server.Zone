using EventHorizon.Monitoring.Model;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace EventHorizon.Monitoring.ApplicationInsights.Telemetry
{
    public class NodeNameFilter : ITelemetryInitializer
    {
        private readonly MonitoringServerConfiguration _serverConfig;

        public NodeNameFilter(
            IOptions<MonitoringServerConfiguration> serverConfig
        )
        {
            _serverConfig = serverConfig.Value;
        }

        public void Initialize(
            ITelemetry telemetry
        )
        {
            var name = $"{_serverConfig.ServerName} ({_serverConfig.Host})";
            telemetry.Context.Cloud.RoleName = name;
            telemetry.Context.Cloud.RoleInstance = name;

            var internalContext = telemetry.Context.GetInternalContext();
            if (string.IsNullOrEmpty(internalContext.NodeName))
            {
                internalContext.NodeName = $"{_serverConfig.ServerName} ({_serverConfig.Host})";
            }
            if (!telemetry.Context.GlobalProperties.ContainsKey("HOST"))
            {
                telemetry.Context.GlobalProperties.Add(
                    "HOST",
                    _serverConfig.Host
                );
            }
        }
    }
}