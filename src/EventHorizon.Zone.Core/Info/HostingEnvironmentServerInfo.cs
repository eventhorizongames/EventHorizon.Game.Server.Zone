namespace EventHorizon.Zone.Core.Info
{
    using EventHorizon.Zone.Core.Model.Info;
    using Microsoft.AspNetCore.Hosting;
    using System;
    using System.IO;

    public class HostingEnvironmentServerInfo 
        : ServerInfo
    {
        public string FileSystemTempPath { get; }
        public string AssembliesPath { get; }
        public string AppDataPath { get; }
        public string SystemPath { get; }
        public string SystemBackupPath { get; }
        public string AdminPath { get; }
        public string PluginsPath { get; }
        public string I18nPath { get; }
        public string ClientPath { get; }
        public string ClientScriptsPath { get; }
        public string ClientEntityPath { get; }
        public string ServerPath { get; }
        public string ServerScriptsPath { get; }
        public string CoreMapPath { get; }

        public HostingEnvironmentServerInfo(
            IHostingEnvironment hostingEnvironment
        )
        {
            FileSystemTempPath = GenerateFileSystemTempPath();
            AssembliesPath = GenerateAssembliesPath();
            AppDataPath = GenerateAppDataPath(
                hostingEnvironment
            );
            SystemPath = GenerateSystemPath(
                hostingEnvironment
            );
            SystemBackupPath = GenerateSystemBackupPath(
                hostingEnvironment
            );
            AdminPath = GenerateAdminPath(
                hostingEnvironment
            );
            PluginsPath = GeneratePluginsPath(
                hostingEnvironment
            );
            I18nPath = GenerateI18nPath(
                hostingEnvironment
            );
            ClientPath = GenerateClientPath(
                hostingEnvironment
            );
            ClientScriptsPath = GenerateClientScriptsPath(
                hostingEnvironment
            );
            ClientEntityPath = GenerateClientEntityPath(
                hostingEnvironment
            );
            ServerPath = GenerateServerPath(
                hostingEnvironment
            );
            ServerScriptsPath = GenerateServerScriptsPath(
                hostingEnvironment
            );
            CoreMapPath = GenerateCoreMapPath(
                hostingEnvironment
            );
        }

        private string GenerateFileSystemTempPath() => Path.Combine(
            Path.DirectorySeparatorChar.ToString(),
            "temp"
        );

        private string GenerateAssembliesPath() 
            => AppDomain.CurrentDomain.BaseDirectory;

        private string GenerateAppDataPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data"
            );
        }
        private string GenerateSystemPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "System"
            );
        }
        private string GenerateSystemBackupPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "__Backup__"
            );
        }
        private string GenerateAdminPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Admin"
            );
        }
        private string GeneratePluginsPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Plugins"
            );
        }
        private string GenerateI18nPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "I18n"
            );
        }
        private string GenerateClientPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Client"
            );
        }
        private string GenerateClientScriptsPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Client",
                "Scripts"
            );
        }
        private string GenerateClientEntityPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Client",
                "Entity"
            );
        }
        private string GenerateServerPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Server"
            );
        }
        private string GenerateServerScriptsPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Server",
                "Scripts"
            );
        }
        private string GenerateCoreMapPath(
            IHostingEnvironment hostingEnvironment
        )
        {
            return Path.Combine(
                hostingEnvironment.ContentRootPath,
                "App_Data",
                "Map"
            );
        }
    }
}