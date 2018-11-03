using EventHorizon.Game.Server.Zone.External.Info;
using Microsoft.AspNetCore.Hosting;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Core.Info
{
    public class ZoneServerInfo : ServerInfo
    {
        private string _pluginsPath;
        private string _assetsPath;

        public string PluginsPath => _pluginsPath;
        public string AssetsPath => _assetsPath;

        public ZoneServerInfo(IHostingEnvironment hostingEnvironment)
        {
            _pluginsPath = GeneratePluginsPath(hostingEnvironment);
            _assetsPath = GenerateAssetsPath(hostingEnvironment);
        }

        private string GeneratePluginsPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Plugins"
            );
        }
        private string GenerateAssetsPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Assets"
            );
        }
    }
}