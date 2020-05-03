namespace EventHorizon.Zone.Core.Map.Model
{
    using EventHorizon.Zone.Core.Model.Map;

    public class ZoneMapMesh : IMapMesh
    {
        public string HeightMapUrl { get; set; }
        public string Light { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Subdivisions { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public bool Updatable { get; set; }
        public bool IsPickable { get; set; }
        public MapMeshMaterial Material { get; set; }
    }
}