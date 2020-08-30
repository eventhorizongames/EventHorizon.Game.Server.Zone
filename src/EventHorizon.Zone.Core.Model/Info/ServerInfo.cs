namespace EventHorizon.Zone.Core.Model.Info
{
    public interface ServerInfo
    {
        /// <summary>
        /// This should return a path on the physical File System that temporary files can be saved to. 
        /// </summary>
        /// <value>Path on file system temporary files can be saved to.</value>
        string FileSystemTempPath { get; }
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