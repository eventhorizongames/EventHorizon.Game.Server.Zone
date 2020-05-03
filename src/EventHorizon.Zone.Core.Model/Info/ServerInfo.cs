namespace EventHorizon.Zone.Core.Model.Info
{
    public interface ServerInfo
    {
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