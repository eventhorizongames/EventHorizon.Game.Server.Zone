namespace EventHorizon.Zone.System.AssetServer.Backup;

public record UploadAssetServerBackupArtifactResult
{
    public string Service { get; } 
    public string Url { get; }

    public UploadAssetServerBackupArtifactResult(
        string service,
        string url
    )
    {
        Service = service;
        Url = url;
    }
}
