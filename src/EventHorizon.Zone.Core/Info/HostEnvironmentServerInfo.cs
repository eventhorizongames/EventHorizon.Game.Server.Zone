namespace EventHorizon.Zone.Core.Info
{
    using System;
    using System.IO;

    using EventHorizon.Zone.Core.Model.Info;

    using Microsoft.Extensions.Hosting;

    public class HostEnvironmentServerInfo
        : ServerInfo
    {
        public string RootPath { get; }
        public string FileSystemTempPath { get; }
        public string AssembliesPath { get; }
        public string GeneratedPath { get; }
        public string AppDataPath { get; }
        public string SystemsPath { get; }
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

        public HostEnvironmentServerInfo(
            IHostEnvironment hostEnvironment
        )
        {
            RootPath = hostEnvironment.ContentRootPath;
            FileSystemTempPath = GenerateFileSystemTempPath();
            AssembliesPath = GenerateAssembliesPath();
            GeneratedPath = GenerateGeneratedPath(
                hostEnvironment
            );
            AppDataPath = GenerateAppDataPath(
                hostEnvironment
            );
            SystemsPath = GenerateSystemsPath(
                hostEnvironment
            );
            SystemBackupPath = GenerateSystemBackupPath(
                hostEnvironment
            );
            AdminPath = GenerateAdminPath(
                hostEnvironment
            );
            PluginsPath = GeneratePluginsPath(
                hostEnvironment
            );
            I18nPath = GenerateI18nPath(
                hostEnvironment
            );
            ClientPath = GenerateClientPath(
                hostEnvironment
            );
            ClientScriptsPath = GenerateClientScriptsPath(
                hostEnvironment
            );
            ClientEntityPath = GenerateClientEntityPath(
                hostEnvironment
            );
            ServerPath = GenerateServerPath(
                hostEnvironment
            );
            ServerScriptsPath = GenerateServerScriptsPath(
                hostEnvironment
            );
            CoreMapPath = GenerateCoreMapPath(
                hostEnvironment
            );
        }

        private string GenerateFileSystemTempPath()
        {
            var tempPath = Path.Combine(
                Path.DirectorySeparatorChar.ToString(),
                "temp"
            );
            if (!Directory.Exists(
                tempPath
            ))
            {
                Directory.CreateDirectory(
                    tempPath
                );
            }
            return tempPath;
        }

        private string GenerateAssembliesPath()
            => AppDomain.CurrentDomain.BaseDirectory;

        private string GenerateGeneratedPath(
                IHostEnvironment hostEnvironment
        ) => Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "_generated"
        );

        private string GenerateAppDataPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data"
            );
        }

        private string GenerateSystemsPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "Systems_Data"
            );
        }

        private string GenerateSystemBackupPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "__Backup__"
            );
        }
        private string GenerateAdminPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Admin"
            );
        }
        private string GeneratePluginsPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Plugins"
            );
        }
        private string GenerateI18nPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "I18n"
            );
        }
        private string GenerateClientPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Client"
            );
        }
        private string GenerateClientScriptsPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Client",
                "Scripts"
            );
        }
        private string GenerateClientEntityPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Client",
                "Entity"
            );
        }
        private string GenerateServerPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Server"
            );
        }
        private string GenerateServerScriptsPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Server",
                "Scripts"
            );
        }
        private string GenerateCoreMapPath(
            IHostEnvironment hostEnvironment
        )
        {
            return Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Map"
            );
        }
    }
}
