namespace EventHorizon.Zone.System.AssetServer.Model;

using EventHorizon.Game.Server.Zone;

internal class AssetServerSystemSettingsModel
    : AssetServerSystemSettings
{
    public string Server { get; } = string.Empty;
    public string PublicServer { get; } = string.Empty;

    public AssetServerSystemSettingsModel(
        SystemAssetServerOptions options
    )
    {
        Server = options.Server;
        PublicServer = options.PublicServer;
    }
}
