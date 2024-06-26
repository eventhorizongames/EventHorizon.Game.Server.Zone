namespace EventHorizon.Zone.Core.Model.Info;

public interface ServerInfo
{
    /// <summary>
    /// The Application Installation Path.
    /// </summary>
    public string RootPath { get; }
    /// <summary>
    /// This should return a path on the physical File System that temporary files can be saved to. 
    /// </summary>
    string FileSystemTempPath { get; }
    /// <summary>
    /// This should return a path to a temporary directory.
    /// <note>Is not as long term storage location.</note>
    /// </summary>
    string TempPath { get; }
    /// <summary>
    /// The location of where all Application Assemblies are located.
    /// </summary>
    string AssembliesPath { get; }
    /// <summary>
    /// The Path to save Server generated content.
    /// </summary>
    string GeneratedPath { get; }
    string SystemsPath { get; }
    string SystemBackupPath { get; }
    string AppDataPath { get; }
    string PluginsPath { get; }
    string I18nPath { get; }
    string AdminPath { get; }
    string PlayerPath { get; }
    string ServerPath { get; }
    string ServerScriptsPath { get; }
    string ClientPath { get; }
    string ClientScriptsPath { get; }
    string ClientEntityPath { get; }
    string CoreMapPath { get; }
}
