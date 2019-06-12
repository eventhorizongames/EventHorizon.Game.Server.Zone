namespace EventHorizon.Game.Server.Zone.External.Info
{
    public interface ServerInfo
    {
        string AdminPath { get; }
        string PluginsPath { get; }
        string AssetsPath { get; }
        string ScriptsPath { get; }
        string ServerPath { get; }
        string SystemsPath { get; }
        string EntityPath { get; }
    }
}