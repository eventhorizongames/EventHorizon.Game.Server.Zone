namespace EventHorizon.Game.Server.Zone.External.Info
{
    public interface ServerInfo
    {
        string AppDataPath { get; }
        string PluginsPath { get; }
        string I18nPath { get; }
        string AdminPath { get; }
        string ServerPath { get; }
        string ServerScriptsPath { get; }
        string ClientPath { get; }
        string ClientScriptsPath { get; }
        string ClientEntityPath { get; }
    }
}