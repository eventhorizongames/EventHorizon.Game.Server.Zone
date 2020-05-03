namespace EventHorizon.Zone.Core.Map.Model
{
    using EventHorizon.Zone.Core.Model.Asset;

    public struct MapMeshMaterial
    {
        public string AssetPath { get; set; }
        public string ShaderId { get; set; }
        public string Shader { get; set; }

        public double SandLimit { get; set; }
        public double RockLimit { get; set; }
        public double SnowLimit { get; set; }

        public TextureAsset GroundTexture { get; set; }
        public TextureAsset GrassTexture { get; set; }
        public TextureAsset SnowTexture { get; set; }
        public TextureAsset SandTexture { get; set; }
        public TextureAsset RockTexture { get; set; }
        public TextureAsset BlendTexture { get; set; }
    }
}
