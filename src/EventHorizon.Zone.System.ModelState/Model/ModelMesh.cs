namespace EventHorizon.Zone.System.ModelState
{
    public struct ModelMesh
    {

        public static readonly ModelMesh DEFAULT = new ModelMesh
        {
            Type = "GLTF",
            AssetId = "DEFAULT_MESH"
        };

        public string Type { get; set; }
        public string AssetId { get; set; }

        public bool IsValid()
        {
            return Type != null
                && !string.IsNullOrEmpty(AssetId);
        }
    }
}