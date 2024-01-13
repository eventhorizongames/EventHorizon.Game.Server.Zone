namespace EventHorizon.Zone.System.ModelState;

public struct ModelMesh
{
    public static readonly ModelMesh DEFAULT = new()
    {
        AssetId = "DEFAULT_MESH"
    };

    public string AssetId { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(AssetId);
    }
}
