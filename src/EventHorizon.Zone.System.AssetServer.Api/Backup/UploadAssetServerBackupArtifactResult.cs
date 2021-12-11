namespace EventHorizon.Zone.System.AssetServer.Backup;

public record UploadAssetServerBackupArtifactResult
{
    public string Service { get; } 
    public string Path { get; }

    public UploadAssetServerBackupArtifactResult(
        string service,
        string path
    )
    {
        Service = service;
        Path = path;
    }
}
