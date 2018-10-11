using EventHorizon.Game.Server.Zone.External.Info;
using Microsoft.AspNetCore.Hosting;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Core.Info
{
    public class ZoneServerInfo : ServerInfo
    {
        private string _pluginsPath;

        public string PluginsPath => _pluginsPath;

        public ZoneServerInfo(IHostingEnvironment hostingEnvironment)
        {
            _pluginsPath = GeneratePluginsPath(hostingEnvironment);
        }

        private string GeneratePluginsPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Plugins"
            );
        }
    }
}