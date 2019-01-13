namespace EventHorizon.Plugin.Zone.System.Model.Model
{
    public struct ModelMesh
    {

        public static readonly ModelMesh DEFAULT = new ModelMesh
        {
            Type = "GLTF",
            Path = "/Assets/Meshes/",
            File = "Default_Mesh.glb",
            HeightOffset = 0,
        };

        public string Type { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
        public float HeightOffset { get; set; }

        public bool IsValid()
        {
            return Type != null
                && Path != null
                && File != null;
        }
    }
}