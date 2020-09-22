namespace EventHorizon.Zone.Core.Model.Info
{
    public interface ServerInfo
    {
        /// <summary>
        /// This should return a path on the physical File System that temporary files can be saved to. 
        /// </summary>
        string FileSystemTempPath { get; }
        /// <summary>
        /// The location of where all Application Assemblies are located.
        /// </summary>
        string AssembliesPath { get; }
        /// <summary>
        /// The Path to save Server generated content.
        /// </summary>
        string GeneratedPath { get; }
        string SystemPath { get; }
        string SystemBackupPath { get; }
        string AppDataPath { get; }
        string PluginsPath { get; }
        string I18nPath { get; }
        string AdminPath { get; }
        string ServerPath { get; }
        string ServerScriptsPath { get; }
        string ClientPath { get; }
        string ClientScriptsPath { get; }
        string ClientEntityPath { get; }
        string CoreMapPath { get; }
    }
}