namespace EventHorizon.Game.Server.Zone.External.Info
{
    public interface ServerInfo
    {
        string PluginsPath { get; }
        string AssetsPath { get; }
        string ScriptsPath { get; }
        string SystemsPath { get; }
        string EntityPath { get; }
    }
}