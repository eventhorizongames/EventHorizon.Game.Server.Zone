namespace EventHorizon.Zone.System.AssetServer.Export;

public record UploadAssetServerExportArtifactResult
{
    public string Service { get; }
    public string Path { get; }

    public UploadAssetServerExportArtifactResult(
        string service,
        string path
    )
    {
        Service = service;
        Path = path;
    }
}
