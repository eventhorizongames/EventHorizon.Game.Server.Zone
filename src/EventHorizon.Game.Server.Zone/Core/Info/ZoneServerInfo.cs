using EventHorizon.Game.Server.Zone.External.Info;
using Microsoft.AspNetCore.Hosting;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Core.Info
{
    public class ZoneServerInfo : ServerInfo
    {
        private string _adminPath;
        private string _pluginsPath;
        private string _assetsPath;
        private string _scriptsPath;
        private string _serverPath;
        private string _systemsPath;
        private string _entityPath;

        public string AdminPath => _adminPath;
        public string PluginsPath => _pluginsPath;
        public string AssetsPath => _assetsPath;
        public string ScriptsPath => _scriptsPath;
        public string ServerPath => _serverPath;
        public string SystemsPath => _systemsPath;
        public string EntityPath => _entityPath;

        public ZoneServerInfo(IHostingEnvironment hostingEnvironment)
        {
            _adminPath = GenerateAdminPath(hostingEnvironment);
            _pluginsPath = GeneratePluginsPath(hostingEnvironment);
            _assetsPath = GenerateAssetsPath(hostingEnvironment);
            _scriptsPath = GenerateScriptsPath(hostingEnvironment);
            _serverPath = GenerateServerPath(hostingEnvironment);
            _systemsPath = GenerateSystemsPath(hostingEnvironment);
            _entityPath = GenerateEntityPath(hostingEnvironment);
        }

        private string GenerateAdminPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Admin"
            );
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
        private string GenerateScriptsPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Assets",
                "Scripts"
            );
        }
        private string GenerateServerPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Server"
            );
        }
        private string GenerateSystemsPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Systems"
            );
        }
        private string GenerateEntityPath(IHostingEnvironment hostingEnvironment)
        {
            return IOPath.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Entity"
            );
        }
    }
}