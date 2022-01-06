namespace EventHorizon.Zone.System.AssetServer.Export;

public record UploadAssetServerExportArtifactResult
{
    public string Service { get; }
    public string Url { get; }

    public UploadAssetServerExportArtifactResult(
        string service,
        string url
    )
    {
        Service = service;
        Url = url;
    }
}
