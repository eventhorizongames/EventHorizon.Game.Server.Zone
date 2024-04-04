namespace EventHorizon.Zone.Core.Info;

using System;
using System.IO;

using EventHorizon.Zone.Core.Model.Info;

using Microsoft.Extensions.Hosting;

public class HostEnvironmentServerInfo
    : ServerInfo
{
    public string RootPath { get; }
    public string FileSystemTempPath { get; }
    public string TempPath { get; }
    public string AssembliesPath { get; }
    public string GeneratedPath { get; }
    public string AppDataPath { get; }
    public string SystemsPath { get; }
    public string SystemBackupPath { get; }
    public string AdminPath { get; }
    public string PlayerPath { get; }
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
        TempPath = GenerateTempPath();
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
        PlayerPath = GeneratePlayerPath(
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

    private static string GenerateFileSystemTempPath()
    {
        var tempPath = Path.Combine(
            Path.DirectorySeparatorChar.ToString(),
            "temp"
        );
        Directory.CreateDirectory(
            tempPath
        );
        return tempPath;
    }

    private static string GenerateTempPath()
        => GenerateFileSystemTempPath();

    private static string GenerateAssembliesPath()
        => AppDomain.CurrentDomain.BaseDirectory;

    private static string GenerateGeneratedPath(
            IHostEnvironment hostEnvironment
    ) => Path.Combine(
        hostEnvironment.ContentRootPath,
        "App_Data",
        "_generated"
    );

    private static string GenerateAppDataPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data"
        );
    }

    private static string GenerateSystemsPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "Systems_Data"
        );
    }

    private static string GenerateSystemBackupPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "__Backup__"
        );
    }
    private static string GenerateAdminPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "Admin"
        );
    }
    private static string GeneratePluginsPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "Plugins"
        );
    }
    private static string GenerateI18nPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "I18n"
        );
    }
    private static string GenerateClientPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "Client"
        );
    }
    private static string GenerateClientScriptsPath(
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
    private static string GenerateClientEntityPath(
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

    private static string GeneratePlayerPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "Player"
        );
    }
    
    private static string GenerateServerPath(
        IHostEnvironment hostEnvironment
    )
    {
        return Path.Combine(
            hostEnvironment.ContentRootPath,
            "App_Data",
            "Server"
        );
    }
    private static string GenerateServerScriptsPath(
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
    private static string GenerateCoreMapPath(
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
