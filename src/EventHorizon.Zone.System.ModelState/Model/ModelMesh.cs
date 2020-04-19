namespace EventHorizon.Zone.System.ModelState
{
    public struct ModelMesh
    {
        public static readonly ModelMesh DEFAULT = new ModelMesh
        {
            AssetId = "DEFAULT_MESH"
        };

        public string AssetId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AssetId);
        }
    }
}